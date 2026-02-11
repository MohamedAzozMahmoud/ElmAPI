using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using MediatR;

namespace Elm.Application.SettingsHandler
{
    public sealed class GetSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, Result<List<SettingsDto>>>
    {
        private readonly ISettingsService _settingsService;
        public GetSettingsQueryHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public async Task<Result<List<SettingsDto>>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
        {
            var result = await _settingsService.GetAllSettingsAsync();
            if (result.IsSuccess && result.Data != null)
            {
                return Result<List<SettingsDto>>.Success(result.Data, "Settings retrieved successfully.");
            }
            else
            {
                return Result<List<SettingsDto>>.Failure(result.Message ?? "Failed to retrieve settings.");
            }
        }
    }
}
