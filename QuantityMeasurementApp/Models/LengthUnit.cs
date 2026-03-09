namespace QuantityMeasurementApp.Models
{ /// <summary>
  ///   Enumeration representing different length units in the Quantity Measurement application.
  ///   This enumeration defines the supported length units, such as Feet and Inch, which can
  /// be used in conjunction with measurement classes and services to facilitate operations on various units of measurement within the application.
  /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inch
    }

    public static class LengthUnitExtensions
    { // Extension method to convert LengthUnit to a factor for conversion to feet
        public static double ToFeetFactor(LengthUnit unit)
        { // Return the conversion factor to feet based on the specified LengthUnit
           switch (unit)
        {
    case LengthUnit.Feet:
        return 1.0;

    case LengthUnit.Inch:
        return 1.0 / 12.0;

    default:
        throw new ArgumentException("Unsupported unit");
}
        }
    }
}