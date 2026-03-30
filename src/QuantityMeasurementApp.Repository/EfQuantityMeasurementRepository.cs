using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository.Data;

namespace QuantityMeasurementApp.Repository
{
    public class EfQuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public EfQuantityMeasurementRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(
            QuantityMeasurementEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            await _dbContext.QuantityMeasurements.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<QuantityMeasurementEntity>> GetAllAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext
                .QuantityMeasurements.OrderByDescending(item => item.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<QuantityMeasurementEntity>> GetByOperationTypeAsync(
            OperationType operationType,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbContext
                .QuantityMeasurements.Where(item => item.OperationType == operationType)
                .OrderByDescending(item => item.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }
    }
}
