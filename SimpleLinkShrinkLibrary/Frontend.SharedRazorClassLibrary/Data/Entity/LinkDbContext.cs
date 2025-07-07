using Microsoft.EntityFrameworkCore;

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Data.Entity
{
    public class LinkDbContext : DbContext
    {
        public DbSet<Shortlink> Shortlinks { get; set; }

        public LinkDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
