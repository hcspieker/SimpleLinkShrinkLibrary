using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.BackgroundServices
{
    public class InitializeDbService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitializeDbService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShortlinkDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
