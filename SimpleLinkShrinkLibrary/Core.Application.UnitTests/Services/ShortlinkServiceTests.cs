using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Core.Application.Exceptions;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Core.Application.Services;
using SimpleLinkShrinkLibrary.Core.Application.UnitTests.Exceptions;
using SimpleLinkShrinkLibrary.Core.Application.Util;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;
using System.Linq.Expressions;

namespace SimpleLinkShrinkLibrary.Core.Application.UnitTests.Services
{
    public class ShortlinkServiceTests
    {
        private readonly IShortlinkService _service;

        private readonly Mock<IShortlinkRepository> _repositoryMock;
        private readonly Mock<IRandomStringGenerator> _generatorMock;
        private readonly Mock<IShortlinkDefaultValues> _defaultValuesMock;

        public ShortlinkServiceTests()
        {
            _repositoryMock = new Mock<IShortlinkRepository>();
            _generatorMock = new Mock<IRandomStringGenerator>();
            _defaultValuesMock = new Mock<IShortlinkDefaultValues>();
            var loggerMock = new NullLogger<ShortlinkService>();

            _service = new ShortlinkService(_repositoryMock.Object, _generatorMock.Object,
                _defaultValuesMock.Object, loggerMock);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_GetByAlias")]
        public async Task GetByAlias_LoadExistingAlias_ReturnsExpectedShortlink()
        {
            // Arrange
            var alias = "existing-alias";
            var expectedShortlink = new Shortlink
            {
                Id = 123,
                Alias = alias,
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .ReturnsAsync(expectedShortlink);

            // Act
            var result = await _service.GetByAlias(alias);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedShortlink, result);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_CreateShortlink_ReturnsShortlink()
        {
            // Arrange
            var targetUrl = "https://example.com";
            var alias = "random-alias";
            Shortlink? createdShortlink = null;

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string>());

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Returns(alias);

            _repositoryMock.Setup(x => x.CreateAsync(It.Is<Shortlink>(y => y.TargetUrl == targetUrl && y.Alias == alias)))
                .ReturnsAsync((Shortlink s) =>
                {
                    createdShortlink = s;
                    return s;
                });

            // Act
            var result = await _service.Create(targetUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdShortlink, result);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_CreateShortlink_UsesUtcDate()
        {
            // Arrange
            var targetUrl = "https://example.com";
            var alias = "random-alias";
            Shortlink? createdShortlink = null;

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string>());

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Returns(alias);

            _repositoryMock.Setup(x => x.CreateAsync(It.Is<Shortlink>(y => y.TargetUrl == targetUrl && y.Alias == alias)))
                .ReturnsAsync((Shortlink s) =>
                {
                    createdShortlink = s;
                    return s;
                });

            // Act
            var result = await _service.Create(targetUrl);

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.ExpirationDate!.Value.Kind);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_CreateFails_ReturnsCreateShortlinkException()
        {
            // Arrange
            var targetUrl = "https://example.com";
            var alias = "random-alias";

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string>());

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Returns(alias);

            _repositoryMock.Setup(x => x.CreateAsync(It.Is<Shortlink>(y => y.TargetUrl == targetUrl && y.Alias == alias)))
                .ThrowsAsync(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<CreateShortlinkException>(async () =>
            {
                await _service.Create(targetUrl);
            });
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_GenerateRandomStringFails_ReturnsCreateShortlinkException()
        {
            // Arrange
            var targetUrl = "https://example.com";

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string>());

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Throws(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<CreateShortlinkException>(async () =>
            {
                await _service.Create(targetUrl);
            });
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_ListFails_ReturnsCreateShortlinkException()
        {
            // Arrange
            var targetUrl = "https://example.com";

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .Throws(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<CreateShortlinkException>(async () =>
            {
                await _service.Create(targetUrl);
            });
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_CreateShortlink_RetryAliasDueCollision()
        {
            // Arrange
            var targetUrl = "https://example.com";
            var alias1 = "random-alias1";
            var alias2 = "random-alias2";
            var tryCount = 0;

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string> { alias1 });

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Returns(() =>
                {
                    tryCount++;
                    return tryCount == 1 ? alias1 : alias2;
                });

            _repositoryMock.Setup(x => x.CreateAsync(It.Is<Shortlink>(y => y.TargetUrl == targetUrl && y.Alias == alias2)))
                .ReturnsAsync((Shortlink s) => s);

            // Act
            var result = await _service.Create(targetUrl);

            // Assert
            _generatorMock.Verify(x => x.GenerateRandomString(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Create")]
        public async Task Create_CreateShortlink_AbortDueTooManyCollisions()
        {
            // Arrange
            var targetUrl = "https://example.com";
            var alias = "random-alias1";
            var tryCount = 0;
            var throwAfterAttempts = ServiceConstants.MaxAliasGenerationRetries + 1;

            _repositoryMock.Setup(x => x.ListUsedAliasesAsync())
                .ReturnsAsync(new List<string> { alias });

            _generatorMock.Setup(x => x.GenerateRandomString(It.IsAny<int>()))
                .Returns(() => ++tryCount <= throwAfterAttempts ? alias : throw new DummyException());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CreateShortlinkException>(async () =>
            {
                await _service.Create(targetUrl);
            });

            Assert.IsType<TooManyRetriesException>(exception.InnerException);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Delete")]
        public async Task Delete_ExistingShortlink_CallsGetByIdAsync()
        {
            // Arrange
            var id = 123;
            var expectedShortlink = new Shortlink
            {
                Id = id,
                Alias = "existing-alias",
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == id)))
                .ReturnsAsync(expectedShortlink);

            // Act
            await _service.Delete(id);

            // Assert
            _repositoryMock.Verify(x => x.GetByIdAsync(It.Is<int>(y => y == id)), Times.Once);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Delete")]
        public async Task Delete_ExistingShortlink_CallsDeleteAsync()
        {
            // Arrange
            var id = 123;
            var expectedShortlink = new Shortlink
            {
                Id = id,
                Alias = "existing-alias",
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedShortlink);

            _repositoryMock.Setup(x => x.DeleteAsync(It.Is<Shortlink>(y => y == expectedShortlink)))
                .Returns(Task.CompletedTask);

            // Act
            await _service.Delete(id);

            // Assert
            _repositoryMock.Verify(x => x.DeleteAsync(It.Is<Shortlink>(y => y == expectedShortlink)), Times.Once);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Delete")]
        public async Task Delete_MissingShortlink_DoesntHandleException()
        {
            // Arrange
            var id = 123;
            var expectedShortlink = new Shortlink
            {
                Id = id,
                Alias = "existing-alias",
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == id)))
                .ThrowsAsync(new EntryNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<EntryNotFoundException>(async () =>
              {
                  await _service.Delete(id);
              });
        }

        [Fact]
        [Trait("Category", "ShortlinkService_Delete")]
        public async Task Delete_ErrorWhileDeleting_ThrowsDeleteShortlinkException()
        {
            // Arrange
            var id = 123;
            var expectedShortlink = new Shortlink
            {
                Id = id,
                Alias = "existing-alias",
                TargetUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedShortlink);

            _repositoryMock.Setup(x => x.DeleteAsync(It.Is<Shortlink>(y => y == expectedShortlink)))
                .ThrowsAsync(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<DeleteShortlinkException>(async () =>
            {
                await _service.Delete(id);
            });
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_NoEntries_DoesntCallDelete()
        {
            // Arrange
            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .ReturnsAsync(new List<Shortlink>());

            // Act
            await _service.DeleteExpiredShortlinks();

            // Assert
            _repositoryMock.Verify(
                x => x.DeleteRangeAsync(It.IsAny<IEnumerable<Shortlink>>()),
                Times.Never);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_MultipleEntriesNoneExpired_DoesntCallDelete()
        {
            // Arrange
            var shortlinks = new List<Shortlink>
            {
                new Shortlink { Id = 123, Alias = "alias1", TargetUrl = "https://example.com/1", ExpirationDate = DateTime.UtcNow.AddDays(1) },
                new Shortlink { Id = 234, Alias = "alias2", TargetUrl = "https://example.com/2", ExpirationDate = DateTime.UtcNow.AddHours(5) },
                new Shortlink { Id = 345, Alias = "alias3", TargetUrl = "https://example.com/3", ExpirationDate = null } // No expiration
            };

            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .Returns((Expression<Func<Shortlink, bool>> query) =>
                    Task.FromResult((IReadOnlyList<Shortlink>)shortlinks.AsQueryable().Where(query).ToList()));

            // Act
            await _service.DeleteExpiredShortlinks();

            // Assert
            _repositoryMock.Verify(
                x => x.DeleteRangeAsync(It.IsAny<IEnumerable<Shortlink>>()),
                Times.Never);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_MultipleEntriesOneExpired_CallsDelete()
        {
            // Arrange
            var idToDelete = 123;
            var shortlinks = new List<Shortlink>
            {
                new Shortlink { Id = idToDelete, Alias = "alias1", TargetUrl = "https://example.com/1", ExpirationDate = DateTime.UtcNow.AddDays(-1) },
                new Shortlink { Id = 234, Alias = "alias2", TargetUrl = "https://example.com/2", ExpirationDate = DateTime.UtcNow.AddMinutes(1) },
                new Shortlink { Id = 345, Alias = "alias3", TargetUrl = "https://example.com/3", ExpirationDate = null } // No expiration
            };

            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .Returns((Expression<Func<Shortlink, bool>> query) =>
                    Task.FromResult((IReadOnlyList<Shortlink>)shortlinks.AsQueryable().Where(query).ToList()));

            _repositoryMock.Setup(
                x => x.DeleteRangeAsync(It.Is<IEnumerable<Shortlink>>(y => y.Count() == 1 && y.Any(z => z.Id == idToDelete))))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteExpiredShortlinks();

            // Assert
            _repositoryMock.Verify(
                x => x.DeleteRangeAsync(It.Is<IEnumerable<Shortlink>>(y => y.Count() == 1 && y.Any(z => z.Id == idToDelete))),
                Times.Once);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_MultipleEntriesAllExpired_CallsDelete()
        {
            // Arrange
            var shortlinks = new List<Shortlink>
            {
                new Shortlink { Id = 123, Alias = "alias1", TargetUrl = "https://example.com/1", ExpirationDate = DateTime.UtcNow.AddDays(-1) },
                new Shortlink { Id = 234, Alias = "alias2", TargetUrl = "https://example.com/2", ExpirationDate = DateTime.UtcNow.AddHours(-5) },
                new Shortlink { Id = 345, Alias = "alias3", TargetUrl = "https://example.com/3", ExpirationDate = DateTime.UtcNow.AddSeconds(-1) } // No expiration
            };

            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .Returns((Expression<Func<Shortlink, bool>> query) =>
                    Task.FromResult((IReadOnlyList<Shortlink>)shortlinks.AsQueryable().Where(query).ToList()));

            _repositoryMock.Setup(x => x.DeleteRangeAsync(It.Is<IEnumerable<Shortlink>>(y => y.Count() == 3)))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteExpiredShortlinks();

            // Assert
            _repositoryMock.Verify(
                x => x.DeleteRangeAsync(It.Is<IEnumerable<Shortlink>>(y => y.Count() == 3)),
                Times.Once);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_UnexpectedErrorWhileLoading_ThrowsDeleteShortlinkException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .ThrowsAsync(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<DeleteShortlinkException>(_service.DeleteExpiredShortlinks);
        }

        [Fact]
        [Trait("Category", "ShortlinkService_DeleteExpiredShortlinks")]
        public async Task DeleteExpiredShortlinks_UnexpectedErrorWhileDeleting_ThrowsDeleteShortlinkException()
        {
            // Arrange
            var idToDelete = 123;
            var shortlinks = new List<Shortlink>
            {
                new Shortlink { Id = idToDelete, Alias = "alias1", TargetUrl = "https://example.com/1", ExpirationDate = DateTime.UtcNow.AddDays(-1) }
            };

            _repositoryMock.Setup(x => x.ListAllAsync(It.IsAny<Expression<Func<Shortlink, bool>>>()))
                .Returns((Expression<Func<Shortlink, bool>> query) =>
                    Task.FromResult((IReadOnlyList<Shortlink>)shortlinks.AsQueryable().Where(query).ToList()));

            _repositoryMock.Setup(x => x.DeleteRangeAsync(It.IsAny<IEnumerable<Shortlink>>()))
                .ThrowsAsync(new DummyException());

            // Act & Assert
            await Assert.ThrowsAsync<DeleteShortlinkException>(async () =>
            {
                await _service.DeleteExpiredShortlinks();
            });
        }
    }
}
