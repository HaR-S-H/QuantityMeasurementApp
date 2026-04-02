using System;

namespace QuantityMeasurementApp.Models.DTOs
{
    public class OperationHistoryDTO
    {
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
