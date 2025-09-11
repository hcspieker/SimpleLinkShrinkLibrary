using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Application.Util;

namespace SimpleLinkShrinkLibrary.Core.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IShortlinkService, ShortlinkService>();
            services.AddSingleton<IRandomStringGenerator, RandomStringGenerator>();

            return services;
        }
    }
}
