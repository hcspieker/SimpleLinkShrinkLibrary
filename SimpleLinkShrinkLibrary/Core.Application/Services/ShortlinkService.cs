using Microsoft.Extensions.Logging;
using SimpleLinkShrinkLibrary.Core.Application.Configuration;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Core.Application.Util;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;

namespace SimpleLinkShrinkLibrary.Core.Application.Services
{
    public class ShortlinkService : IShortlinkService
    {
        private readonly IShortlinkRepository _repository;
        private readonly IRandomStringGenerator _randomStringGenerator;
        private readonly IShortlinkDefaultValues _defaultValues;
        private readonly ILogger<ShortlinkService> _logger;

        public ShortlinkService(IShortlinkRepository repository, IRandomStringGenerator randomStringGenerator,
            IShortlinkDefaultValues shortlinkDefaultValues, ILogger<ShortlinkService> logger)
        {
            _repository = repository;
            _randomStringGenerator = randomStringGenerator;
            _defaultValues = shortlinkDefaultValues;
            _logger = logger;
        }

        public async Task<Shortlink> GetByAlias(string alias)
        {
            try
            {
                return await _repository.GetAsync(x => x.Alias == alias);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while searching a shortlink for the alias {alias}", alias);

                throw new RetrieveShortlinkException($"Error while searching a shortlink for the alias {alias}");
            }
        }

        public async Task<Shortlink> Create(string targetUrl)
        {
            var alias = await GenerateUniqueAlias();
            var expirationDate = DateTime.Now.Add(_defaultValues.ExpirationSpan);

            var entity = new Shortlink { TargetUrl = targetUrl, Alias = alias, ExpirationDate = expirationDate };

            await _repository.CreateAsync(entity);

            return entity;
        }

        private async Task<string> GenerateUniqueAlias()
        {
            var currentlyUsedAliases = await _repository.ListUsedAliasesAsync();

            var generatedAlias = _randomStringGenerator.GenerateRandomString(_defaultValues.AliasLength);

            while (currentlyUsedAliases.Any(x => x == generatedAlias))
            {
                _logger.LogWarning("Generated alias collided with existing value {alias}", generatedAlias);
                generatedAlias = _randomStringGenerator.GenerateRandomString(_defaultValues.AliasLength);
            }

            return generatedAlias;
        }

        public async Task Delete(int id)
        {
            var shortlink = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(shortlink);
        }

        public async Task DeleteExpiredShortlinks()
        {
            var linksToDelete = await _repository.ListAllAsync(x => x.ExpirationDate != null && x.ExpirationDate < DateTime.Now);

            if (linksToDelete.Any())
                await _repository.DeleteRangeAsync(linksToDelete);
        }
    }
}
