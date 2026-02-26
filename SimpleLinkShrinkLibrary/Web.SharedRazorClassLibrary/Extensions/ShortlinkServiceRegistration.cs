using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Core.Application;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Configuration;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Extensions
{
    public static class ShortlinkServiceRegistration
    {
        public static IServiceCollection EnableShortlinks(this IServiceCollection services, IConfiguration configuration, bool enableReverseProxySupport = false)
        {
            services.Configure<ShortlinkSettings>(configuration.GetRequiredSection(nameof(ShortlinkSettings)));
            services.AddSingleton<IShortlinkDefaultValues, ShortlinkDefaultValues>();

            services.AddApplicationServices();

            if (enableReverseProxySupport)
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto |
                        ForwardedHeaders.XForwardedHost;

                    // Trust forwarded headers from any proxy (Docker network)
#if NET10_0_OR_GREATER
                    options.KnownIPNetworks.Clear();
#else
                    options.KnownNetworks.Clear();
#endif
                    options.KnownProxies.Clear();
                });
            }

            return services;
        }
    }
}
