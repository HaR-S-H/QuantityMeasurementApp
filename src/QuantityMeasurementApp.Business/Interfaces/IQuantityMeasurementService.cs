using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Convert(QuantityDTO source, string targetUnit);
        bool Compare(QuantityDTO first, QuantityDTO second);
        QuantityDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        double Divide(QuantityDTO a, QuantityDTO b);

        Task<bool> CompareAsync(
            QuantityDTO first,
            QuantityDTO second,
            CancellationToken cancellationToken = default
        );
        Task<QuantityDTO> ConvertAsync(
            QuantityDTO source,
            string targetUnit,
            CancellationToken cancellationToken = default
        );
        Task<QuantityDTO> AddAsync(
            QuantityDTO a,
            QuantityDTO b,
            string? targetUnit = null,
            CancellationToken cancellationToken = default
        );
        Task<QuantityDTO> SubtractAsync(
            QuantityDTO a,
            QuantityDTO b,
            string? targetUnit = null,
            CancellationToken cancellationToken = default
        );
        Task<double> DivideAsync(
            QuantityDTO a,
            QuantityDTO b,
            CancellationToken cancellationToken = default
        );
        Task<IReadOnlyList<OperationHistoryDto>> GetHistoryAsync(
            OperationType? operationType = null,
            CancellationToken cancellationToken = default
        );
    }
}
