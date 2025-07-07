using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.BackgroundServices;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Configuration;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Util;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection EnableShortlinks(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ShortlinkDbConnectionString");

            if (connectionString == null)
                throw new ShortlinkDbConnectionStringNotFoundException("Connection string 'ShortlinkDbConnectionString' not found.");

            services.AddDbContext<LinkDbContext>(o => o.UseSqlite(connectionString));

            services.Configure<ShortLinkSettings>(configuration.GetRequiredSection(nameof(ShortLinkSettings)));

            services.AddControllersWithViews();

            services.AddSingleton<IRandomStringGenerator, RandomStringGenerator>();
            services.AddScoped<IRepository, Repository>();

            services.AddHostedService<InitDbService>();
            services.AddHostedService<CleanupExpiredShortlinksService>();

            return services;
        }
    }
}
