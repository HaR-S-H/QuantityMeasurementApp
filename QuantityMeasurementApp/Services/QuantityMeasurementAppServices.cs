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
          public static bool AreFeetEqual(double value1, double value2)
        {   // Create two Feet instances with the provided values
             Feet feet1 = new Feet(value1);
             Feet feet2 = new Feet(value2);
            // Compare the two Feet instances for equality and return the result
            return feet1.Equals(feet2);
        }
          // Method to compare two Inches instances for equality based on their values  
        public static bool AreInchesEqual(double value1, double value2)
        {  // Create two Inches instances with the provided values
            Inches inch1 = new Inches(value1);
            Inches inch2 = new Inches(value2);
            // Compare the two Inches instances for equality and return the result
            return inch1.Equals(inch2);
        }
    }
}