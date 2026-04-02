using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        private static readonly TimeZoneInfo IndiaTimeZone = ResolveIndiaTimeZone();

        public Guid Id { get; }
        public Guid? UserId { get; }
        public string Description { get; }
        public bool IsError { get; }
        public string ErrorMessage { get; }
        public DateTime CreatedAt { get; }

        public QuantityMeasurementEntity(string description, Guid? userId = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Description = description;
            IsError = false;
            ErrorMessage = string.Empty;
            CreatedAt = GetCurrentIndiaTime();
        }

        public QuantityMeasurementEntity(string description, string errorMessage, Guid? userId = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Description = description;
            IsError = true;
            ErrorMessage = errorMessage;
            CreatedAt = GetCurrentIndiaTime();
        }

        private QuantityMeasurementEntity(
            Guid id,
            Guid? userId,
            string description,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            Id = id;
            UserId = userId;
            Description = description;
            IsError = isError;
            ErrorMessage = errorMessage;
            CreatedAt = createdAt;
        }

        public static QuantityMeasurementEntity Rehydrate(
            Guid id,
            Guid? userId,
            string description,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            return new QuantityMeasurementEntity(id, userId, description, isError, errorMessage, createdAt);
        }

        private static DateTime GetCurrentIndiaTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndiaTimeZone);
        }

        private static TimeZoneInfo ResolveIndiaTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
        }
    }
}
