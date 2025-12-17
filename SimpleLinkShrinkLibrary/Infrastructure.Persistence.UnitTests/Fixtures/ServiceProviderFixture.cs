using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.UnitTests.Fixtures
{
    public class ServiceProviderFixture
    {
        public IServiceCollection Services { get; }
        private const string InMemoryDatabaseName = "TestDatabase_CAB9D038-5DAA-4620-BFF1-152E8D837103";

        public ServiceProviderFixture()
        {
            Services = new ServiceCollection();
            Services.AddDbContext<ShortlinkDbContext>(options =>
            {
                options.UseInMemoryDatabase(InMemoryDatabaseName);
            });
            Services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }
    }
}
