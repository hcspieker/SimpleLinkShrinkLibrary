using SimpleLinkShrinkLibrary.Core.Application.Util;

namespace SimpleLinkShrinkLibrary.Core.Application.UnitTests.Util
{
    public class RandomStringGeneratorTests
    {
        private readonly RandomStringGenerator _generator;

        public RandomStringGeneratorTests()
        {
            _generator = new RandomStringGenerator();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void GenerateRandomString_LengthX_ReturnsStringOfLengthX(int length)
        {
            // Act
            var result = _generator.GenerateRandomString(length);

            // Assert
            Assert.Equal(length, result.Length);
        }

        [Fact]
        public void GenerateRandomString_MultipleCalls_ReturnsDifferentStrings()
        {
            // Arrange
            int length = 15;
            var amountOfRuns = 100;
            var results = new HashSet<string>();

            // Act
            for (int i = 0; i < amountOfRuns; i++)
            {
                var result = _generator.GenerateRandomString(length);
                results.Add(result);
            }

            // Assert
            Assert.Equal(amountOfRuns, results.Count); // Expecting all generated strings to be unique
        }

        [Fact]
        public void GenerateRandomString_Length0_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int length = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _generator.GenerateRandomString(length));
        }

        [Fact]
        public void GenerateRandomString_LengthNegative_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int length = -5;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _generator.GenerateRandomString(length));
        }
    }
}
