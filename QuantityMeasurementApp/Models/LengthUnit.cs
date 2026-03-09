namespace QuantityMeasurementApp.Models
{ /// <summary>
  ///   Enumeration representing different length units in the Quantity Measurement application.
  ///   This enumeration defines the supported length units, such as Feet and Inch, which can
  /// be used in conjunction with measurement classes and services to facilitate operations on various units of measurement within the application.
  /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeter
    }

    public static class LengthUnitMethods
    {
        // Conversion factors
        private const double INCHES_PER_FOOT = 12.0;
        private const double FEET_PER_YARD = 3.0;
        private const double CENTIMETERS_PER_FOOT = 30.48;

        // Convert a value in this unit to feet (base unit)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / INCHES_PER_FOOT,
                LengthUnit.Yard => value * FEET_PER_YARD,
                LengthUnit.Centimeter => value / CENTIMETERS_PER_FOOT,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // Convert a value in feet (base unit) to this unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.Feet => baseValue,
                LengthUnit.Inch => baseValue * INCHES_PER_FOOT,
                LengthUnit.Yard => baseValue / FEET_PER_YARD,
                LengthUnit.Centimeter => baseValue * CENTIMETERS_PER_FOOT,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}