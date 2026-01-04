namespace Infrastructure.Data.Configurations;

public class UserPictureConfigurations : IEntityTypeConfiguration<UserPicture>
{
    public void Configure(EntityTypeBuilder<UserPicture> builder)
    {
        builder.HasOne<Picture>()
            .WithOne()
            .HasForeignKey<UserPicture>(up => up.PictureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
