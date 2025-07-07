using SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data
{
    public interface IRepository
    {
        Task<Shortlink> Create(string targetUrl);
        Task<Shortlink> Get(string alias);
        Task Delete(int id);
    }
}
