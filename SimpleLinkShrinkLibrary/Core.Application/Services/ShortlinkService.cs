using Microsoft.Extensions.Logging;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
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
            var alias = await GenerateUniqueAlias();
            var expirationDate = DateTime.Now.Add(defaultValues.ExpirationSpan);

            var entity = new Shortlink { TargetUrl = targetUrl, Alias = alias, ExpirationDate = expirationDate };

            await repository.CreateAsync(entity);

            return entity;
        }

        private async Task<string> GenerateUniqueAlias()
        {
            var currentlyUsedAliases = await repository.ListUsedAliasesAsync();

            var generatedAlias = randomStringGenerator.GenerateRandomString(defaultValues.AliasLength);

            while (currentlyUsedAliases.Any(x => x == generatedAlias))
            {
                logger.LogWarning("Generated alias collided with existing value {alias}", generatedAlias);
                generatedAlias = randomStringGenerator.GenerateRandomString(defaultValues.AliasLength);
            }

            return generatedAlias;
        }

        public async Task Delete(int id)
        {
            var shortlink = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(shortlink);
        }

        public async Task DeleteExpiredShortlinks()
        {
            var linksToDelete = await repository.ListAllAsync(x => x.ExpirationDate != null && x.ExpirationDate < DateTime.Now);

            if (linksToDelete.Any())
                await repository.DeleteRangeAsync(linksToDelete);
        }
    }
}
