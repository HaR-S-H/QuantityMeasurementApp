using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository.Data
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements => Set<QuantityMeasurementEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.RequestPayload).IsRequired();
                entity.Property(item => item.CreatedAtUtc).IsRequired();
            });
        }
    }
}
