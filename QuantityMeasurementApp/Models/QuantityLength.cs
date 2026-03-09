
namespace QuantityMeasurementApp.Models
{
     /// <summary>
  ///  Class representing a quantity of length in the Quantity Measurement application.
  /// This class provides methods to convert the quantity to the base unit (feet) and compare it with other quantity lengths for equality.
  ///  The QuantityLength class can be used in conjunction with other measurement classes, such as Feet, to facilitate operations on various units of measurement within the application.
  /// </summary>
    public class QuantityLength : IEquatable<QuantityLength>
    {
        private readonly double _value;
        private readonly LengthUnit _unit;

    
        public QuantityLength(double value, LengthUnit unit)
        {
            _value = value;
            _unit = unit;
        }

        // Convert to base unit (feet)
        private double ConvertToBaseUnit()
        {
            return _value * LengthUnitExtensions.ToFeetFactor(_unit);
        }

        // UC3 → Compare two lengths using base unit
        public bool Equals(QuantityLength? other)
        {
            if (other is null)
                return false;

            return ConvertToBaseUnit().CompareTo(other.ConvertToBaseUnit()) == 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is QuantityLength other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
    }
}