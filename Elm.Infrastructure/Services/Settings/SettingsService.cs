using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Elm.Infrastructure.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly AppDbContext context;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "SystemQuotas";
        private readonly IOptions<SettingsOptions> _options;
        public SettingsService(AppDbContext _context, IMemoryCache cache, IOptions<SettingsOptions> options)
        {
            context = _context;
            _cache = cache;
            _options = options;
        }
        public async Task<int> GetMaxQuestionsAsync()
        {
            return (await GetSystemQuotasAsync()).MaxQuestions;
        }

        public async Task<long> GetMaxStorageBytesAsync()
        {
            return (await GetSystemQuotasAsync()).MaxStorageBytes;
        }
        private async Task<(int MaxQuestions, long MaxStorageBytes)> GetSystemQuotasAsync()
        {
            return await _cache.GetOrCreateAsync(CacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1); // تبقى في الذاكرة ساعة
                var settings = await context.Settings.ToListAsync();

                var maxQ = int.Parse(settings.FirstOrDefault(s => s.Key == "MaxQuestions")?.Value ?? _options.Value.MaxQuestions.ToString());
                var maxS = long.Parse(settings.FirstOrDefault(s => s.Key == "MaxStorage")?.Value ?? _options.Value.MaxStorageBytes.ToString());

                return (maxQ, maxS);
            });
        }

        public void ClearCache()
        {
            _cache.Remove(CacheKey);
        }

        public async Task<Result<bool>> AddSettingsAsync(string key, string value)
        {
            var existing = await context.Settings.FirstOrDefaultAsync(s => s.Key == key);
            if (existing != null)
            {
                return Result<bool>.Failure($"Setting with key '{key}' already exists.");
            }
            var setting = new Domain.Entities.Settings
            {
                Key = key,
                Value = value
            };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();
            ClearCache();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateSettingsAsync(string key, string value)
        {
            var existing = await context.Settings.FirstOrDefaultAsync(s => s.Key == key);
            if (existing == null)
            {
                return Result<bool>.Failure($"Setting with key '{key}' does not exist.");
            }
            existing.Value = value;
            context.Settings.Update(existing);
            await context.SaveChangesAsync();
            ClearCache();
            return Result<bool>.Success(true);
        }

        public async Task<Result<List<SettingsDto>>> GetAllSettingsAsync()
        {
            var settingsDto = await context.Settings
                .AsNoTracking()
                .Select(s => new SettingsDto(s.Key, s.Value))
                .ToListAsync();
            return Result<List<SettingsDto>>.Success(settingsDto);
        }
    }
}
