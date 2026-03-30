using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Repository
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly QuantityMeasurementCacheRepository _instance = new();
        private readonly List<QuantityMeasurementEntity> _cache = new();
        private readonly object _lock = new();

        public static QuantityMeasurementCacheRepository Instance => _instance;

        private QuantityMeasurementCacheRepository() { }

        public Task SaveAsync(
            QuantityMeasurementEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            lock (_lock)
            {
                _cache.Add(entity);
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<QuantityMeasurementEntity>> GetAllAsync(
            CancellationToken cancellationToken = default
        )
        {
            lock (_lock)
            {
                return Task.FromResult<IReadOnlyList<QuantityMeasurementEntity>>(_cache.ToList());
            }
        }

        public Task<IReadOnlyList<QuantityMeasurementEntity>> GetByOperationTypeAsync(
            OperationType operationType,
            CancellationToken cancellationToken = default
        )
        {
            lock (_lock)
            {
                var result = _cache.Where(item => item.OperationType == operationType).ToList();
                return Task.FromResult<IReadOnlyList<QuantityMeasurementEntity>>(result);
            }
        }
    }
}
