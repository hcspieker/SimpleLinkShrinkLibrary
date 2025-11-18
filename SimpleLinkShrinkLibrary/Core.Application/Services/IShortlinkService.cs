using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Core.Application.Services
{
    public interface IShortlinkService
    {
        Task<Shortlink> GetByAlias(string alias);
        Task<Shortlink> Create(string targetUrl);
        Task Delete(int id);
        Task DeleteExpiredShortlinks();
    }
}
