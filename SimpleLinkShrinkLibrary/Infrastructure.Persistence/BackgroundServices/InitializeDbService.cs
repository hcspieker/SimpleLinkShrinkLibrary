using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.BackgroundServices
{
    public class InitializeDbService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ShortlinkDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken: stoppingToken);
        }
    }
}
