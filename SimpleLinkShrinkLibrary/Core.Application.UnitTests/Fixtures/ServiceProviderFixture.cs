using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;

namespace SimpleLinkShrinkLibrary.Core.Application.UnitTests.Fixtures
{
    public class ServiceProviderFixture
    {
        public IServiceCollection Services { get; }

        public Mock<IShortlinkRepository> RepositoryMock { get; }
        public Mock<IShortlinkDefaultValues> DefaultValuesMock { get; }

        public ServiceProviderFixture()
        {
            RepositoryMock = new Mock<IShortlinkRepository>();
            DefaultValuesMock = new Mock<IShortlinkDefaultValues>();

            Services = new ServiceCollection();
            Services.AddSingleton(RepositoryMock.Object);
            Services.AddSingleton(DefaultValuesMock.Object);
            Services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }
    }
}
