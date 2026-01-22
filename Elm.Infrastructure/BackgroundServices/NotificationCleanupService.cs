using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elm.Infrastructure.BackgroundServices;
public class NotificationCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationCleanupService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // التشغيل كل 24 ساعة

    public NotificationCleanupService(IServiceScopeFactory scopeFactory, ILogger<NotificationCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("خدمة تنظيف الإشعارات بدأت العمل.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("بدء عملية تنظيف الإشعارات القديمة...");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var cutoffDate = DateTime.UtcNow.AddDays(-3);

                    // استخدام ExecuteDeleteAsync للحذف المباشر لضمان أعلى أداء
                    int deletedCount = await context.Notifications
                        .Where(n => n.IsRead && n.CreatedAt < cutoffDate)
                        .ExecuteDeleteAsync(stoppingToken);

                    _logger.LogInformation("تم حذف {Count} إشعار قديم بنجاح.", deletedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء تنظيف الإشعارات.");
            }

            // الانتظار لمدة 24 ساعة قبل المرة القادمة
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}