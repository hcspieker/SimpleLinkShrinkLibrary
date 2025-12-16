using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Application.UnitTests.Fixtures;
using SimpleLinkShrinkLibrary.Core.Application.Util;

namespace SimpleLinkShrinkLibrary.Core.Application.UnitTests
{
    public class ServiceCollectionTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceProviderFixture _fixture;

        public ServiceCollectionTests(ServiceProviderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddApplicationServices_ShortlinkService_RegistersServiceCorrectly()
        {
            // Arrange
            _fixture.Services.AddApplicationServices();
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var shortlinkService = serviceProvider.GetService<IShortlinkService>();

            // Assert
            Assert.NotNull(shortlinkService);
        }

        [Fact]
        public void AddApplicationServices_RandomStringGenerator_RegistersServiceCorrectly()
        {
            // Arrange
            _fixture.Services.AddApplicationServices();
            var serviceProvider = _fixture.Services.BuildServiceProvider();

            // Act
            var randomStringGenerator = serviceProvider.GetService<IRandomStringGenerator>();

            // Assert
            Assert.NotNull(randomStringGenerator);
        }
    }
}
