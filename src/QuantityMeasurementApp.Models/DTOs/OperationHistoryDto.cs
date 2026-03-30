using System;

namespace QuantityMeasurementApp.Models.DTOs
{
    public class OperationHistoryDto
    {
        public int Id { get; set; }
        public OperationType OperationType { get; set; }
        public string RequestPayload { get; set; } = string.Empty;
        public string? ResponsePayload { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
