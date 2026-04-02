using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    /// <summary>
    /// SQL-backed repository implementation using EF Core.
    /// </summary>
    public sealed class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public QuantityMeasurementDatabaseRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            // Persist a single audit/event entry.
            _dbContext.QuantityMeasurementOperations.Add(entity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll()
        {
            // Read-only query optimized with AsNoTracking.
            return _dbContext
                .QuantityMeasurementOperations.AsNoTracking()
                .OrderByDescending(entity => entity.CreatedAt)
                .ToList();
        }

        public IEnumerable<QuantityMeasurementEntity> GetByUserId(Guid userId)
        {
            return _dbContext
                .QuantityMeasurementOperations.AsNoTracking()
                .Where(entity => entity.UserId == userId)
                .OrderByDescending(entity => entity.CreatedAt)
                .ToList();
        }
    }
}
