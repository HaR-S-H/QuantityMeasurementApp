using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    /// <summary>
    /// EF Core DbContext for quantity measurement persistence.
    /// </summary>
    public sealed class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurementOperations =>
            Set<QuantityMeasurementEntity>();

        public DbSet<UserEntity> Users => Set<UserEntity>();

        public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                // Keep mapping explicit so schema remains stable across migrations.
                entity.ToTable("QuantityMeasurementOperations", "dbo");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.IsError).IsRequired();
                entity.Property(e => e.ErrorMessage).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UserId).IsRequired(false);

                entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_QuantityMeasurementOperations_CreatedAt");
                entity.HasIndex(e => e.UserId).HasDatabaseName("IX_QuantityMeasurementOperations_UserId");

                entity
                    .HasOne<UserEntity>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users", "dbo");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Users_Email");
                entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_Users_CreatedAt");
            });

            modelBuilder.Entity<RevokedTokenEntity>(entity =>
            {
                entity.ToTable("RevokedTokens", "dbo");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.TokenId).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ExpiresAtUtc).IsRequired();
                entity.Property(e => e.RevokedAtUtc).IsRequired();

                entity.HasIndex(e => e.TokenId).IsUnique().HasDatabaseName("IX_RevokedTokens_TokenId");
                entity
                    .HasIndex(e => e.ExpiresAtUtc)
                    .HasDatabaseName("IX_RevokedTokens_ExpiresAtUtc");
            });
        }
    }
}