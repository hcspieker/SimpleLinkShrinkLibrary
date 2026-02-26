using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Core.Application;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Configuration;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Extensions
{
    public static class ShortlinkServiceRegistration
    {
        public static IServiceCollection EnableShortlinks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ShortlinkSettings>(configuration.GetRequiredSection(nameof(ShortlinkSettings)));
            services.AddSingleton<IShortlinkDefaultValues, ShortlinkDefaultValues>();

            services.AddApplicationServices();

            return services;
        }
    }
}
