using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleLinkShrinkLibrary.Core.Application.Services;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.BackgroundServices
{
    public class CleanupExpiredShortlinksService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CleanupExpiredShortlinksService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            var firstRun = true;

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                if (firstRun)
                {
                    timer.Period = TimeSpan.FromHours(1);
                    firstRun = false;
                }

                if (stoppingToken.IsCancellationRequested)
                    return;

                try
                {
                    await DeleteExpiredShortlinks();
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Error while deleting expired shortlinks.");
                }
            }
        }

        private async Task DeleteExpiredShortlinks()
        {
            // todo: handle exceptions
            using var scope = serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ShortlinkService>();
            await service.DeleteExpiredShortlinks();
        }
    }
}
