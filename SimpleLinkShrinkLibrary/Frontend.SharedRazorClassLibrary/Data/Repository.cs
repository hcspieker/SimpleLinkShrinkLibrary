using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Configuration;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Exceptions;
using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Util;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data
{
    public class Repository : IRepository
    {
        private readonly LinkDbContext _context;
        private readonly IRandomStringGenerator _randomStringGenerator;
        private readonly IOptionsMonitor<ShortLinkSettings> _optionsMonitor;

        private ShortLinkSettings _settings => _optionsMonitor.CurrentValue;

        public Repository(LinkDbContext context, IRandomStringGenerator randomStringGenerator, IOptionsMonitor<ShortLinkSettings> optionsMonitor)
        {
            _context = context;
            _randomStringGenerator = randomStringGenerator;
            _optionsMonitor = optionsMonitor;
        }

        public async Task<Shortlink> Create(string targetUrl)
        {
            var entity = new Shortlink
            {
                TargetUrl = targetUrl,
                Alias = _randomStringGenerator.GenerateRandomString(_settings.LinkAliasLength),
                ExpirationDate = DateTime.Now.Add(_settings.LinkExpirationSpan)
            };

            var amountOfRetries = 5;
            var retry = 0;
            while (await _context.Shortlinks.AnyAsync(x => x.Alias == entity.Alias))
            {
                if (retry < amountOfRetries)
                    throw new CreateShortlinkException($"Error while generating a new shortlink alias.");

                retry++;
                entity.Alias = _randomStringGenerator.GenerateRandomString(_settings.LinkAliasLength);
            }

            await _context.Shortlinks.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Shortlink> Get(string alias)
        {
            try
            {
                return await _context.Shortlinks.SingleAsync(x => x.Alias == alias);
            }
            catch (InvalidOperationException)
            {
                throw new ShortlinkNotFoundException($"No shortlink was found for the alias {alias}.");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entry = await _context.Shortlinks.SingleAsync(x => x.Id == id);

                _context.Shortlinks.Remove(entry);
                await _context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw new ShortlinkNotFoundException($"No shortlink was found for the id {id}.");
            }
        }
    }
}
