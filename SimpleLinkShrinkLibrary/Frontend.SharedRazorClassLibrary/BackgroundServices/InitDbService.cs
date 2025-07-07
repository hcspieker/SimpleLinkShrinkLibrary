using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.BackgroundServices
{
    public class InitDbService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitDbService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LinkDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
