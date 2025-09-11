using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.BackgroundServices;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.Repositories;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection EnableBasePersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IShortlinkRepository, ShortlinkRepository>();

            services.AddHostedService<InitializeDbService>();
            services.AddHostedService<CleanupExpiredShortlinksService>();

            return services;
        }
    }
}
