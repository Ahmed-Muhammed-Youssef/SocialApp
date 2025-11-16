namespace Infrastructure.Data.Configurations;

public class DirectChatConfigurations : IEntityTypeConfiguration<DirectChat>
{
    public void Configure(EntityTypeBuilder<DirectChat> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(c => c.Messages)
               .WithOne()
               .HasForeignKey(m => m.ChatId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.User1Id).IsRequired();
        builder.Property(c => c.User2Id).IsRequired();

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(m => m.User1Id)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(m => m.User2Id)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
