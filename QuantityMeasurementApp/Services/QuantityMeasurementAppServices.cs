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
        // Method to compare two Feet instances for equality
        public bool AreEqual(Feet feet1, Feet feet2)
        {// Handle null cases
            if (feet1 == null || feet2 == null)
                return false;
                // Use the overridden Equals method in the Feet class for value-based comparison
            return feet1.Equals(feet2);
        }
    }
}