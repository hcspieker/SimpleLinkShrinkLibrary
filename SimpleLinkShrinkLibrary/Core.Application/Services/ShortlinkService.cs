using Microsoft.Extensions.Logging;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Core.Application.Exceptions;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Core.Application.Util;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;

namespace SimpleLinkShrinkLibrary.Core.Application.Services
{
    public class ShortlinkService(
        IShortlinkRepository repository,
        IRandomStringGenerator randomStringGenerator,
        IShortlinkDefaultValues defaultValues,
        ILogger<ShortlinkService> logger) : IShortlinkService
    {
        public async Task<Shortlink> GetByAlias(string alias)
        {
            try
            {
                return await repository.GetAsync(x => x.Alias == alias);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while searching a shortlink for the alias {alias}", alias);

                throw new RetrieveShortlinkException($"Error while searching a shortlink for the alias {alias}");
            }
        }

        public async Task<Shortlink> Create(string targetUrl)
        {
            try
            {
                var alias = await GenerateUniqueAlias();
                var expirationDate = DateTime.UtcNow.Add(defaultValues.ExpirationSpan);

                var entity = new Shortlink
                {
                    TargetUrl = targetUrl,
                    Alias = alias,
                    ExpirationDate = expirationDate
                };

                await repository.CreateAsync(entity);

                return entity;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while creating a new Shortlink for target URL {targetUrl}", targetUrl);
                throw new CreateShortlinkException("Error while creating a new Shortlink", e);
            }
        }

        private async Task<string> GenerateUniqueAlias()
        {
            var currentlyUsedAliases = await repository.ListUsedAliasesAsync();

            var generatedAlias = randomStringGenerator.GenerateRandomString(defaultValues.AliasLength);
            var retry = 0;

            while (currentlyUsedAliases.Any(x => x == generatedAlias))
            {
                retry++;
                logger.LogWarning("Generated alias collided with existing value {alias}", generatedAlias);

                if (retry >= ServiceConstants.MaxAliasGenerationRetries)
                {
                    logger.LogError("Max retries reached when generating unique alias");
                    throw new TooManyRetriesException("Max retries reached when generating unique alias");
                }

                logger.LogInformation("Retrying to generate a unique alias. Attempt {retry}", retry);
                generatedAlias = randomStringGenerator.GenerateRandomString(defaultValues.AliasLength);
            }

            return generatedAlias;
        }

        public async Task Delete(int id)
        {
            try
            {
                var shortlink = await repository.GetByIdAsync(id);
                await repository.DeleteAsync(shortlink);
            }
            catch (Exception e) when (e is not EntryNotFoundException)
            {
                logger.LogError(e, "Error while deleting Shortlink with id {id}", id);

                throw new DeleteShortlinkException("Error while deleting Shortlink", e);
            }
        }

        public async Task DeleteExpiredShortlinks()
        {
            try
            {
                var linksToDelete = await repository.ListAllAsync(x => x.ExpirationDate != null && x.ExpirationDate < DateTime.UtcNow);

                if (linksToDelete.Any())
                    await repository.DeleteRangeAsync(linksToDelete);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while deleting expired Shortlinks");

                throw new DeleteShortlinkException("Error while deleting expired Shortlins", e);
            }
        }
    }
}
