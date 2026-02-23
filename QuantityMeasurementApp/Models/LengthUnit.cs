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

    public static class LengthUnitExtensions
    { // Extension method to convert LengthUnit to a factor for conversion to feet
        public static double ToFeetFactor(this LengthUnit unit)
        {
            // Return the conversion factor to feet based on the specified LengthUnit
            return unit switch
            {
                LengthUnit.Feet => 1.0,

                // 12 inches = 1 foot
                LengthUnit.Inch => 1.0 / 12.0,

                // 1 yard = 3 feet
                LengthUnit.Yard => 3.0,

                // 1 cm = 0.393701 inch
                // convert inch → feet
                LengthUnit.Centimeter => 0.393701 / 12.0,
                // Throw an exception for unsupported units
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}