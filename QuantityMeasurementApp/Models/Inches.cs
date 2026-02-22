
namespace QuantityMeasurementApp.Models
{
/// <summary>
/// Class representing measurements in inches for the Quantity Measurement application.
/// This class implements the IEquatable interface to provide value-based equality comparison for measurements in inches
///     and overrides the Equals and GetHashCode methods to ensure that instances of Inches can be compared accurately based on their values.
/// The Inches class can be used in conjunction with other measurement classes, such as Feet, to facilitate operations on various units of measurement within the application.
/// </summary>
    public class Inches : IEquatable<Inches>
    {
        // Private field to store the value of the measurement in inches
        private readonly double _value;

        // Constructor
        public Inches(double value)
        {
            _value = value;
        }
        //  Implementation of the IEquatable interface to compare two Inches instances for equality based on their values
        public bool Equals(Inches? other)
        {
            if (other is null)
                return false;
            // Compare the values of the two Inches instances for equality
            return _value.CompareTo(other._value) == 0;
        }
        //  Override of the Equals method to provide value-based equality comparison for Inches instances
        public override bool Equals(object? obj)
        {
            return obj is Inches other && Equals(other);
        }
            //  Override of the GetHashCode method to ensure that instances of Inches can be used in hash-based collections and compared accurately based on their values
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}