namespace Infrastructure.Data.Identity;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : IdentityDbContext(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.UserId)
                .HasMaxLength(450)
                .IsRequired();

            entity.Property(rt => rt.Token)
                .HasMaxLength(1000)
                .IsRequired();

            entity.HasIndex(rt => rt.Token).IsUnique();

            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
