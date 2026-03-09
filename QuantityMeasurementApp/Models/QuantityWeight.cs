using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight : IEquatable<QuantityWeight>
    {
        private readonly double _value;
        private readonly WeightUnit _unit;
        private const double EPSILON = 1e-6;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid value");
            _value = value;
            _unit = unit;
        }

        public WeightUnit GetUnit() => _unit;
        public double GetValue() => _value;

        public bool Equals(QuantityWeight? other)
        {
            if (other is null)
                return false;
            double thisInKg = _unit.ConvertToBaseUnit(_value);
            double otherInKg = other._unit.ConvertToBaseUnit(other._value);
            return Math.Abs(thisInKg - otherInKg) < EPSILON;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null || obj.GetType() != GetType()) return false;
            return Equals(obj as QuantityWeight);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_unit.ConvertToBaseUnit(_value));
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");
            double valueInKg = _unit.ConvertToBaseUnit(_value);
            double newValue = targetUnit.ConvertFromBaseUnit(valueInKg);
            return new QuantityWeight(newValue, targetUnit);
        }

        public static QuantityWeight Add(QuantityWeight a, QuantityWeight b)
        {
            return Add(a, b, a._unit);
        }

        public static QuantityWeight Add(QuantityWeight a, QuantityWeight b, WeightUnit targetUnit)
        {
            if (a == null || b == null)
                throw new ArgumentException("Operands cannot be null");
            if (double.IsNaN(a._value) || double.IsInfinity(a._value) || double.IsNaN(b._value) || double.IsInfinity(b._value))
                throw new ArgumentException("Operand values must be finite");
            double aInKg = a._unit.ConvertToBaseUnit(a._value);
            double bInKg = b._unit.ConvertToBaseUnit(b._value);
            double sumInKg = aInKg + bInKg;
            double sumInTarget = targetUnit.ConvertFromBaseUnit(sumInKg);
            sumInTarget = Math.Round(sumInTarget, 6);
            return new QuantityWeight(sumInTarget, targetUnit);
        }

        public override string ToString()
        {
            return $"Quantity({_value}, {_unit})";
        }
    }
}
