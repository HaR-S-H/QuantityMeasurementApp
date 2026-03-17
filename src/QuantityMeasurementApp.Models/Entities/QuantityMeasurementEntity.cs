using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; }
        public string Description { get; }
        public bool IsError { get; }
        public string ErrorMessage { get; }

        public QuantityMeasurementEntity(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
            IsError = false;
            ErrorMessage = string.Empty;
        }

        public QuantityMeasurementEntity(string description, string errorMessage)
        {
            Id = Guid.NewGuid();
            Description = description;
            IsError = true;
            ErrorMessage = errorMessage;
        }
    }
}
