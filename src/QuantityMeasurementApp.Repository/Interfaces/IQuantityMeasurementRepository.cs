using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Repository
{
    public interface IQuantityMeasurementRepository
    {
        Task SaveAsync(QuantityMeasurementEntity entity, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<QuantityMeasurementEntity>> GetAllAsync(
            CancellationToken cancellationToken = default
        );
        Task<IReadOnlyList<QuantityMeasurementEntity>> GetByOperationTypeAsync(
            OperationType operationType,
            CancellationToken cancellationToken = default
        );
    }
}
