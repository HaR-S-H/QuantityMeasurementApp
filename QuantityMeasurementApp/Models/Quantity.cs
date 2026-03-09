using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : struct
    {
        private readonly double _value;
        private readonly U _unit;
        private const double EPSILON = 1e-6;

        public Quantity(double value, U unit)
        {
            if (unit.Equals(null))
                throw new ArgumentException("Unit cannot be null");
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid value");
            _value = value;
            _unit = unit;
        }

        public U GetUnit() => _unit;
        public double GetValue() => _value;

        private static Type GetUnitCategory(U unit)
        {
            return unit.GetType();
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null || obj.GetType() != GetType()) return false;
            var other = obj as Quantity<U>;
            if (other == null) return false;
            if (GetUnitCategory(_unit) != GetUnitCategory(other._unit)) return false;
            double thisBase = ConvertToBase(_value, _unit);
            double otherBase = ConvertToBase(other._value, other._unit);
            return Math.Abs(thisBase - otherBase) < EPSILON;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConvertToBase(_value, _unit));
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double valueInBase = ConvertToBase(_value, _unit);
            double newValue = ConvertFromBase(valueInBase, targetUnit);
            return new Quantity<U>(newValue, targetUnit);
        }

        public static Quantity<U> Add(Quantity<U> a, Quantity<U> b)
        {
            return Add(a, b, a._unit);
        }

        public static Quantity<U> Add(Quantity<U> a, Quantity<U> b, U targetUnit)
        {
            if (a == null || b == null)
                throw new ArgumentException("Operands cannot be null");
            if (double.IsNaN(a._value) || double.IsInfinity(a._value) || double.IsNaN(b._value) || double.IsInfinity(b._value))
                throw new ArgumentException("Operand values must be finite");
            if (GetUnitCategory(a._unit) != GetUnitCategory(b._unit))
                throw new ArgumentException("Cannot add quantities of different categories");
            double aInBase = ConvertToBase(a._value, a._unit);
            double bInBase = ConvertToBase(b._value, b._unit);
            double sumInBase = aInBase + bInBase;
            double sumInTarget = ConvertFromBase(sumInBase, targetUnit);
            sumInTarget = Math.Round(sumInTarget, 6);
            return new Quantity<U>(sumInTarget, targetUnit);
        }

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lengthUnit)
                return LengthUnitExtensions.ConvertToBaseUnit(lengthUnit, value);
            if (unit is WeightUnit weightUnit)
                return WeightUnitExtensions.ConvertToBaseUnit(weightUnit, value);
            if (unit is VolumeUnit volumeUnit)
                return VolumeUnitExtensions.ConvertToBaseUnit(volumeUnit, value);
            throw new ArgumentException("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lengthUnit)
                return LengthUnitExtensions.ConvertFromBaseUnit(lengthUnit, baseValue);
            if (unit is WeightUnit weightUnit)
                return WeightUnitExtensions.ConvertFromBaseUnit(weightUnit, baseValue);
            if (unit is VolumeUnit volumeUnit)
                return VolumeUnitExtensions.ConvertFromBaseUnit(volumeUnit, baseValue);
            throw new ArgumentException("Unsupported unit type");
        }

        public override string ToString()
        {
            string unitName = _unit.ToString();
            return $"Quantity({_value}, {unitName})";
        }
    }
}
