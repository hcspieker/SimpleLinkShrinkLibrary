using Microsoft.EntityFrameworkCore;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence
{
    public class ShortlinkDbContext : DbContext
    {
        public DbSet<Shortlink> Shortlinks { get; set; }

        public ShortlinkDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShortlinkDbContext).Assembly);
        }
    }
}
