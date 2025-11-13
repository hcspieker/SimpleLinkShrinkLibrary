using Microsoft.EntityFrameworkCore;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.Repositories
{
    public class ShortlinkRepository(ShortlinkDbContext dbContext) : BaseRepository<Shortlink>(dbContext), IShortlinkRepository
    {
        public async Task<List<string>> ListUsedAliasesAsync()
        {
            return await _dbContext.Shortlinks.Select(x => x.Alias).ToListAsync();
        }
    }
}
