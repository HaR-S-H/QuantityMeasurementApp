using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Service class for quantity measurement operations.
    /// This class provides methods to compare measurements in different units, such as feet, inches,
    /// and can be extended to include additional functionality such as unit conversion and arithmetic operations on measurements.
    /// The AreEqual method compares two Feet instances for equality based on their values, allowing for accurate comparisons of measurements in feet.
    /// The QuantityMeasurementService can be used in conjunction with the Feet class and other measurement classes to facilitate operations on various units of measurement within the application.
    /// </summary>
    public class QuantityMeasurementService
    {
        // Method to compare two Feet instances for equality based on their values
          public static bool AreFeetEqual(double v1, double v2)
        {   // Create two Feet instances with the provided values
            var f1 = new Feet(v1);
            var f2 = new Feet(v2);
            // Compare the two Feet instances for equality and return the result
            return f1.Equals(f2);
        }
          // Method to compare two Inches instances for equality based on their values  
        public static bool AreInchesEqual(double v1, double v2)
        {  // Create two Inches instances with the provided values
            var i1 = new Inches(v1);
            var i2 = new Inches(v2);
            // Compare the two Inches instances for equality and return the result
            return i1.Equals(i2);
        }
    }
}