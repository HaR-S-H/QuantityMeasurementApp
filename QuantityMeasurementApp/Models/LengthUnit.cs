namespace QuantityMeasurementApp.Models
{ /// <summary>
  ///   Enumeration representing different length units in the Quantity Measurement application.
  ///   This enumeration defines the supported length units, such as Feet and Inch, which can
  /// be used in conjunction with measurement classes and services to facilitate operations on various units of measurement within the application.
  /// </summary>
        public enum LengthUnit : int
        {
            Feet,
            Inch,
            Yard,
            Centimeter
        }

        public static class LengthUnitExtensions
        {
            private const double INCHES_PER_FOOT = 12.0;
            private const double FEET_PER_YARD = 3.0;
            private const double CENTIMETERS_PER_FOOT = 30.48;

            public static double GetConversionFactor(this LengthUnit unit)
            {
                return unit switch
                {
                    LengthUnit.Feet => 1.0,
                    LengthUnit.Inch => 1.0 / INCHES_PER_FOOT,
                    LengthUnit.Yard => FEET_PER_YARD,
                    LengthUnit.Centimeter => 1.0 / CENTIMETERS_PER_FOOT,
                    _ => throw new ArgumentException("Unsupported unit")
                };
            }

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

            public static string GetUnitName(this LengthUnit unit)
            {
                return unit.ToString().ToUpper();
            }
        }
}