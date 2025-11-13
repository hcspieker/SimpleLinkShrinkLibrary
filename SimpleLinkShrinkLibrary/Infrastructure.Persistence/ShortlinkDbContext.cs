using Microsoft.EntityFrameworkCore;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence
{
    public class ShortlinkDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Shortlink> Shortlinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShortlinkDbContext).Assembly);
        }
    }
}
