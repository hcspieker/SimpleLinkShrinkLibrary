using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Core.Domain.UnitTests.Entites
{
    public class ShortlinkTests
    {
        [Fact]
        public void Constructor_ExiprationDate_ShouldBeNull()
        {
            // Act
            var shortlink = new Shortlink()
            {
                Alias = "test-alias",
                TargetUrl = "https://example.com"
            };

            // Assert
            Assert.Null(shortlink.ExpirationDate);
        }

        [Fact]
        public void Constructor_Id_ShouldBeZero()
        {
            // Act
            var shortlink = new Shortlink()
            {
                Alias = "test-alias",
                TargetUrl = "https://example.com"
            };

            // Assert
            Assert.Equal(0, shortlink.Id);
        }
    }
}
