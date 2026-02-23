namespace QuantityMeasurementApp.Models
{
    ///<summary>
    /// Represents a measurement in feet.
    /// This class encapsulates the value of a measurement in feet and provides functionality for unit conversion and comparison.
    /// It is designed to be immutable, ensuring that once an instance is created, its value cannot be changed.
    /// The Feet class can be used in conjunction with other measurement classes (e.g., Inches, Meters) to perform conversions and comparisons between different units of measurement.
    /// </summary>
     public class Feet
    {
        // Immutable value representing the measurement in feet 
        private readonly double _value;

        // Constructor
        public Feet(double value)
        {
            _value = value;
        }
         public double Value => _value;

        // Override Equals (value-based equality)
     public override bool Equals(object other)
{
    // Same reference
    if (ReferenceEquals(this, other))
        return true;

    // Null or wrong type
    if (other == null || other.GetType() != typeof(Feet))
        return false;
    // Compare values
    Feet otherFeet = (Feet)other;
    // Use CompareTo for floating-point comparison
    return _value.CompareTo(otherFeet._value) == 0;
}

        // Must override if Equals overridden
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}