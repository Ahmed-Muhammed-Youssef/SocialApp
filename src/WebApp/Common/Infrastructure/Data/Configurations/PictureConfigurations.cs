namespace Infrastructure.Data.Configurations;

public class PictureConfigurations : IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.HasKey(p => p.Id);

        // properties
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Url).IsRequired();
        builder.Property(p => p.Created).IsRequired();
        builder.Property(p => p.PublicId).IsRequired();
    }

}
