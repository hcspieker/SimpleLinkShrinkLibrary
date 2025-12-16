using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.BackgroundServices;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.SqlServer.UnitTests.Fixtures;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.SqlServer.UnitTests
{
    public class ServiceCollectionTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceProviderFixture _fixture;

        public ServiceCollectionTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddApplicationServices_ShortlinkRepository_RegistersServiceCorrectly()
        {
            // Arrange
            _fixture.Services.EnableSqlServerPersistence(_fixture.Configuration);
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var repository = serviceProvider.GetService<IShortlinkRepository>();

            // Assert
            Assert.NotNull(repository);
        }

        [Fact]
        public void AddApplicationServices_HostedService_CheckRegisteredAmount()
        {
            // Arrange
            _fixture.Services.EnableSqlServerPersistence(_fixture.Configuration);
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var services = serviceProvider.GetServices<IHostedService>();

            // Assert
            Assert.NotNull(services);
            Assert.Equal(2, services.Count());
        }

        [Fact]
        public void AddApplicationServices_HostedService_InitializeDbService()
        {
            // Arrange
            _fixture.Services.EnableSqlServerPersistence(_fixture.Configuration);
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var services = serviceProvider.GetServices<IHostedService>();

            // Assert
            Assert.NotNull(services);

            var initDbService = services.OfType<InitializeDbService>().FirstOrDefault();
            Assert.NotNull(initDbService);
        }

        [Fact]
        public void AddApplicationServices_HostedService_CleanupExpiredShortlinksService()
        {
            // Arrange
            _fixture.Services.EnableSqlServerPersistence(_fixture.Configuration);
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var services = serviceProvider.GetServices<IHostedService>();

            // Assert
            Assert.NotNull(services);

            var cleanupService = services.OfType<CleanupExpiredShortlinksService>().FirstOrDefault();
            Assert.NotNull(cleanupService);
        }
    }
}
