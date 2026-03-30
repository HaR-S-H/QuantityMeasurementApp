using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// Business-layer service that orchestrates quantity operations for both
    /// strongly typed domain models and DTO-based application flows.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl()
            : this(QuantityMeasurementCacheRepository.Instance) { }

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        public QuantityDTO Convert(QuantityDTO source, string targetUnit)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.Category switch
                {
                    MeasurementCategory.Length => ConvertTypedToDto<LengthUnit>(source, targetUnit),
                    MeasurementCategory.Weight => ConvertTypedToDto<WeightUnit>(source, targetUnit),
                    MeasurementCategory.Volume => ConvertTypedToDto<VolumeUnit>(source, targetUnit),
                    MeasurementCategory.Temperature => ConvertTypedToDto<TemperatureUnit>(
                        source,
                        targetUnit
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(source)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Conversion failed", ex);
            }
        }

        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(first);
                ArgumentNullException.ThrowIfNull(second);

                EnsureSameCategory(first, second);

                return first.Category switch
                {
                    MeasurementCategory.Length => AreEqualTyped<LengthUnit>(first, second),
                    MeasurementCategory.Weight => AreEqualTyped<WeightUnit>(first, second),
                    MeasurementCategory.Volume => AreEqualTyped<VolumeUnit>(first, second),
                    MeasurementCategory.Temperature => AreEqualTyped<TemperatureUnit>(
                        first,
                        second
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(first)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Comparison failed", ex);
            }
        }

        public QuantityDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => AddTyped<LengthUnit>(a, b, targetUnit),
                    MeasurementCategory.Weight => AddTyped<WeightUnit>(a, b, targetUnit),
                    MeasurementCategory.Volume => AddTyped<VolumeUnit>(a, b, targetUnit),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature addition is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Addition failed", ex);
            }
        }

        public QuantityDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => SubtractTyped<LengthUnit>(a, b, targetUnit),
                    MeasurementCategory.Weight => SubtractTyped<WeightUnit>(a, b, targetUnit),
                    MeasurementCategory.Volume => SubtractTyped<VolumeUnit>(a, b, targetUnit),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature subtraction is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Subtraction failed", ex);
            }
        }

        public double Divide(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => DivideTyped<LengthUnit>(a, b),
                    MeasurementCategory.Weight => DivideTyped<WeightUnit>(a, b),
                    MeasurementCategory.Volume => DivideTyped<VolumeUnit>(a, b),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature division is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Division failed", ex);
            }
        }

        public async Task<bool> CompareAsync(
            QuantityDTO first,
            QuantityDTO second,
            CancellationToken cancellationToken = default
        )
        {
            const OperationType operation = OperationType.Compare;
            var requestPayload = SerializePayload(new { first, second });

            try
            {
                var result = Compare(first, second);
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    SerializePayload(new { result }),
                    true,
                    null,
                    cancellationToken
                );

                return result;
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    null,
                    false,
                    ex.Message,
                    cancellationToken
                );
                throw;
            }
        }

        public async Task<QuantityDTO> ConvertAsync(
            QuantityDTO source,
            string targetUnit,
            CancellationToken cancellationToken = default
        )
        {
            const OperationType operation = OperationType.Convert;
            var requestPayload = SerializePayload(new { source, targetUnit });

            try
            {
                var result = Convert(source, targetUnit);
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    SerializePayload(result),
                    true,
                    null,
                    cancellationToken
                );

                return result;
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    null,
                    false,
                    ex.Message,
                    cancellationToken
                );
                throw;
            }
        }

        public async Task<QuantityDTO> AddAsync(
            QuantityDTO a,
            QuantityDTO b,
            string? targetUnit = null,
            CancellationToken cancellationToken = default
        )
        {
            const OperationType operation = OperationType.Add;
            var requestPayload = SerializePayload(new { a, b, targetUnit });

            try
            {
                var result = Add(a, b, targetUnit);
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    SerializePayload(result),
                    true,
                    null,
                    cancellationToken
                );

                return result;
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    null,
                    false,
                    ex.Message,
                    cancellationToken
                );
                throw;
            }
        }

        public async Task<QuantityDTO> SubtractAsync(
            QuantityDTO a,
            QuantityDTO b,
            string? targetUnit = null,
            CancellationToken cancellationToken = default
        )
        {
            const OperationType operation = OperationType.Subtract;
            var requestPayload = SerializePayload(new { a, b, targetUnit });

            try
            {
                var result = Subtract(a, b, targetUnit);
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    SerializePayload(result),
                    true,
                    null,
                    cancellationToken
                );

                return result;
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    null,
                    false,
                    ex.Message,
                    cancellationToken
                );
                throw;
            }
        }

        public async Task<double> DivideAsync(
            QuantityDTO a,
            QuantityDTO b,
            CancellationToken cancellationToken = default
        )
        {
            const OperationType operation = OperationType.Divide;
            var requestPayload = SerializePayload(new { a, b });

            try
            {
                var result = Divide(a, b);
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    SerializePayload(new { result }),
                    true,
                    null,
                    cancellationToken
                );

                return result;
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                await SaveHistoryAsync(
                    operation,
                    requestPayload,
                    null,
                    false,
                    ex.Message,
                    cancellationToken
                );
                throw;
            }
        }

        public async Task<IReadOnlyList<OperationHistoryDto>> GetHistoryAsync(
            OperationType? operationType = null,
            CancellationToken cancellationToken = default
        )
        {
            var entities = operationType.HasValue
                ? await _repository.GetByOperationTypeAsync(operationType.Value, cancellationToken)
                : await _repository.GetAllAsync(cancellationToken);

            return entities
                .Select(entity => new OperationHistoryDto
                {
                    Id = entity.Id,
                    OperationType = entity.OperationType,
                    RequestPayload = entity.RequestPayload,
                    ResponsePayload = entity.ResponsePayload,
                    IsSuccess = entity.IsSuccess,
                    ErrorMessage = entity.ErrorMessage,
                    CreatedAtUtc = entity.CreatedAtUtc,
                })
                .ToList();
        }

        private static void EnsureSameCategory(QuantityDTO first, QuantityDTO second)
        {
            if (first.Category != second.Category)
                throw new ArgumentException("Both quantities must belong to the same category.");
        }

        private static QuantityDTO ConvertTypedToDto<U>(QuantityDTO source, string targetUnit)
            where U : struct, Enum
        {
            var sourceUnit = ParseUnit<U>(source.Unit);
            var target = ParseUnit<U>(targetUnit);
            var converted = new Quantity<U>(source.Value, sourceUnit).ConvertTo(target);
            return new QuantityDTO(converted.Value, converted.Unit.ToString(), source.Category);
        }

        private static bool AreEqualTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Equals(right);
        }

        private static QuantityDTO AddTyped<U>(
            QuantityDTO first,
            QuantityDTO second,
            string? targetUnit
        )
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));

            Quantity<U> result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Add(right)
                : left.Add(right, ParseUnit<U>(targetUnit));

            return new QuantityDTO(result.Value, result.Unit.ToString(), first.Category);
        }

        private static QuantityDTO SubtractTyped<U>(
            QuantityDTO first,
            QuantityDTO second,
            string? targetUnit
        )
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));

            Quantity<U> result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Subtract(right)
                : left.Subtract(right, ParseUnit<U>(targetUnit));

            return new QuantityDTO(result.Value, result.Unit.ToString(), first.Category);
        }

        private static double DivideTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Divide(right);
        }

        private static U ParseUnit<U>(string unitName)
            where U : struct, Enum
        {
            if (!Enum.TryParse(unitName, ignoreCase: true, out U parsed))
                throw new ArgumentException($"Unsupported unit '{unitName}' for {typeof(U).Name}.");

            return parsed;
        }

        private async Task SaveHistoryAsync(
            OperationType operationType,
            string requestPayload,
            string? responsePayload,
            bool isSuccess,
            string? errorMessage,
            CancellationToken cancellationToken
        )
        {
            var entity = new QuantityMeasurementEntity
            {
                OperationType = operationType,
                RequestPayload = requestPayload,
                ResponsePayload = responsePayload,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                CreatedAtUtc = DateTime.UtcNow,
            };

            await _repository.SaveAsync(entity, cancellationToken);
        }

        private static string SerializePayload(object payload)
        {
            return JsonSerializer.Serialize(payload);
        }
    }
}
