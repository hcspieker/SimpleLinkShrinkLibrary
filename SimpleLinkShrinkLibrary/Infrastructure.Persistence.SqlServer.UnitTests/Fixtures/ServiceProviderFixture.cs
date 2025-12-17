using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.SqlServer.UnitTests.Fixtures
{
    public class ServiceProviderFixture
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public ServiceProviderFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:ShortlinkDbConnectionString", "Server=(localdb)\\mssqllocaldb;Database=ShortlinkTestDb;Trusted_Connection=True;MultipleActiveResultSets=true" }
                })
                .Build();

            Services = new ServiceCollection();
            Services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }
    }
}
