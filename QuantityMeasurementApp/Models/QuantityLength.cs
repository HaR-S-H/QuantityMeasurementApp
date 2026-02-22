
namespace QuantityMeasurementApp.Models
{ /// <summary>
  ///  Class representing a quantity of length in the Quantity Measurement application.
  /// This class provides methods to convert the quantity to the base unit (feet) and compare it with other quantity lengths for equality.
  ///  The QuantityLength class can be used in conjunction with other measurement classes, such as Feet, to facilitate operations on various units of measurement within the application.
  /// </summary>
    public class QuantityLength : IEquatable<QuantityLength>
    { // Properties
        private readonly double _value;
        // Base unit
        private readonly LengthUnit _unit;
        // Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            _value = value;
            _unit = unit;
        }

        // Convert to base unit (feet)
        private double ToFeet()
        {
            return _value * _unit.ToFeetFactor();
        }
        

        public bool Equals(QuantityLength? other)
        {
            if (other is null)
                return false;

            // Convert both to same base unit
            return ToFeet().CompareTo(other.ToFeet()) == 0;
        }
        // Equals method

        public override bool Equals(object? obj)
        {
            return obj is QuantityLength other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ToFeet());
        }
    }
}