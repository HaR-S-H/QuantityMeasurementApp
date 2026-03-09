using System;

namespace QuantityMeasurementApp.Models
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit
    }

    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            // Base unit: Celsius
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return value;
                case TemperatureUnit.Fahrenheit:
                    return (value - 32) * 5.0 / 9.0;
                default:
                    throw new ArgumentException("Unsupported temperature unit");
            }
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            // Base unit: Celsius
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return baseValue;
                case TemperatureUnit.Fahrenheit:
                    return baseValue * 9.0 / 5.0 + 32;
                default:
                    throw new ArgumentException("Unsupported temperature unit");
            }
        }

        public static string GetUnitName(this TemperatureUnit unit)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return "Celsius";
                case TemperatureUnit.Fahrenheit:
                    return "Fahrenheit";
                default:
                    return "Unknown";
            }
        }

        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new NotSupportedException($"Operation '{operation}' is not supported for temperature measurements.");
        }
    }
}
