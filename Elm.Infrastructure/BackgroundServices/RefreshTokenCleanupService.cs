using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elm.Infrastructure.BackgroundServices;

// RefreshToken CleanupService: خدمة تعمل في الخلفية لتنظيف الإشعارات القديمة
public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // التشغيل كل ساعة
    public RefreshTokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("خدمة تنظيف رموز التحديث بدأت العمل.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("بدء عملية تنظيف رموز التحديث منتهية الصلاحية...");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var now = DateTime.UtcNow;
                    // استخدام ExecuteDeleteAsync للحذف المباشر لضمان أعلى أداء
                    int deletedCount = await context.RefreshToken
                        .Where(rt => rt.ExpiresOn < now || rt.RevokedOn != null)
                        .ExecuteDeleteAsync(stoppingToken);
                    _logger.LogInformation("تم حذف {Count} رمز تحديث منتهي الصلاحية بنجاح.", deletedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء تنظيف رموز التحديث.");
            }
            // الانتظار لمدة ساعة قبل المرة القادمة
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
