using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Core.Application.Persistence
{
    public interface IShortlinkRepository : IRepository<Shortlink>
    {
        Task<List<string>> ListUsedAliasesAsync();
    }
}