using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.BackgroundServices
{
    public class CleanupExpiredShortlinksService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<CleanupExpiredShortlinksService> _logger;

        public CleanupExpiredShortlinksService(IServiceScopeFactory serviceScopeFactory, ILogger<CleanupExpiredShortlinksService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            var firstRun = true;

            while (await timer.WaitForNextTickAsync())
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
                    _logger.LogError(exception, "Error while deleting expired shortlinks.");
                }
            }
        }

        private async Task DeleteExpiredShortlinks()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<LinkDbContext>();
            var linksToDelete = dbContext.Shortlinks.Where(x => x.ExpirationDate != null && x.ExpirationDate < DateTime.Now).ToList();

            if (linksToDelete.Any())
            {
                foreach (var link in linksToDelete)
                {
                    dbContext.Shortlinks.Remove(link);
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
