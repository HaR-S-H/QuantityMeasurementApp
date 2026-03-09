
namespace QuantityMeasurementApp.Models
{ /// <summary>
  ///  Class representing a quantity of length in the Quantity Measurement application.
  /// This class provides methods to convert the quantity to the base unit (feet) and compare it with other quantity lengths for equality.
  ///  The QuantityLength class can be used in conjunction with other measurement classes, such as Feet, to facilitate operations on various units of measurement within the application.
  /// </summary>
    public class QuantityLength : IEquatable<QuantityLength>
    {
        // Expose unit and value for testing and comparison
        public LengthUnit GetUnit() => _unit;
        public double GetValue() => _value;
        // Properties
                /// <summary>
                /// Adds two QuantityLength objects and returns the result in the specified target unit.
                /// </summary>
                /// <param name="a">First quantity</param>
                /// <param name="b">Second quantity</param>
                /// <param name="targetUnit">Target unit for the result</param>
                /// <returns>New QuantityLength in the target unit</returns>
                /// <exception cref="ArgumentException">If any argument is invalid</exception>
                public static QuantityLength Add(QuantityLength a, QuantityLength b, LengthUnit targetUnit)
                {
                    if (a == null || b == null)
                        throw new ArgumentException("Operands cannot be null");
                    if (targetUnit == null)
                        throw new ArgumentException("Target unit cannot be null");
                    if (double.IsNaN(a._value) || double.IsInfinity(a._value) || double.IsNaN(b._value) || double.IsInfinity(b._value))
                        throw new ArgumentException("Operand values must be finite");

                    // Convert both to base unit (feet)
                    double aInFeet = a.ToFeet();
                    double bInFeet = b.ToFeet();
                    double sumInFeet = aInFeet + bInFeet;

                    // Convert sum to target unit
                    double sumInTarget = sumInFeet / targetUnit.ToFeetFactor();
                    // Round to 6 decimal places for precision
                    sumInTarget = Math.Round(sumInTarget, 6);
                    return new QuantityLength(sumInTarget, targetUnit);
                }

                /// <summary>
                /// Overload for backward compatibility: returns result in the unit of the first operand.
                /// </summary>
                public static QuantityLength Add(QuantityLength a, QuantityLength b)
                {
                    if (a == null)
                        throw new ArgumentException("First operand cannot be null");
                    return Add(a, b, a._unit);
                }
        private readonly double _value;
        // Base unit
        private readonly LengthUnit _unit;
        // Constructor
      public QuantityLength(double value, LengthUnit unit)
{
    if (unit == null)
        throw new ArgumentException("Unit cannot be null");

    if (double.IsNaN(value) || double.IsInfinity(value))
        throw new ArgumentException("Invalid value");

    _value = value;
    _unit = unit;
}

        // Convert to base unit (feet)
        private double ToFeet()
        {
            return _value * _unit.ToFeetFactor();
        }
        
    // Define a small value for comparison
       private const double EPSILON = 1e-6;
    // Implement the IEquatable interface to compare two QuantityLength instances for equality based on their values and units
    public bool Equals(QuantityLength? other)
    {
        // Check for null and compare the values of the two QuantityLength instances for equality, allowing for a small margin of error (EPSILON) to account for floating-point precision issues
        if (other is null)
            return false;
        // Compare the values of the two QuantityLength instances for equality, allowing for a small margin of error (EPSILON) to account for floating-point precision issues
        return Math.Abs(ToFeet() - other.ToFeet()) < EPSILON;
    }
    // Equals method
        // Override of the Equals method to provide value-based equality comparison for QuantityLength instances
        public override bool Equals(object? obj)
        {
            return obj is QuantityLength other && Equals(other);
        }
    // Override of the GetHashCode method to ensure that instances of QuantityLength can be used in hash-based collections and compared accurately based on their values and units
        public override int GetHashCode()
        {
            return HashCode.Combine(ToFeet());
        }
        public static double Convert(double value, LengthUnit source, LengthUnit target)
{
    // validation
    if (source == null || target == null)
        throw new ArgumentException("Source or target unit cannot be null");

    if (double.IsNaN(value) || double.IsInfinity(value))
        throw new ArgumentException("Value must be finite");

    // convert source → base (feet)
    double valueInFeet = value * source.ToFeetFactor();

    // convert base → target
    return valueInFeet / target.ToFeetFactor();
}
        public double ConvertTo(LengthUnit targetUnit)
{
    return Convert(_value, _unit, targetUnit);
}
        public QuantityLength ConvertToQuantity(LengthUnit targetUnit)
{
    double newValue = ConvertTo(targetUnit);
    return new QuantityLength(newValue, targetUnit);
}
    }
}