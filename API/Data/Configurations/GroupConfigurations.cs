using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class GroupConfigurations : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Name);

            // properties
            builder.Property(g => g.Name).IsRequired();

            // relationships
            builder.HasMany(g => g.Connections)
                    .WithOne();
        }
    }
}
