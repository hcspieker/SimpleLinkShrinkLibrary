using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.Configurations
{
    public class ShortlinkConfiguration : IEntityTypeConfiguration<Shortlink>
    {
        public void Configure(EntityTypeBuilder<Shortlink> builder)
        {
            builder.HasIndex(x => x.Alias)
                .IsUnique();
        }
    }
}
