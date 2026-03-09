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

        // Centralized arithmetic operation enum
        private enum ArithmeticOperation
        {
            Add,
            Subtract,
            Divide
        }

        // Centralized validation for operands and target unit
        private static void ValidateArithmeticOperands(Quantity<U> a, Quantity<U> b, U? targetUnit, bool targetUnitRequired, ArithmeticOperation op)
        {
            if (a == null || b == null)
                throw new ArgumentException("Operands cannot be null");
            if (double.IsNaN(a._value) || double.IsInfinity(a._value) || double.IsNaN(b._value) || double.IsInfinity(b._value))
                throw new ArgumentException("Operand values must be finite");
            if (GetUnitCategory(a._unit) != GetUnitCategory(b._unit))
                throw new ArgumentException($"Cannot {op.ToString().ToLower()} quantities of different categories");
            if (targetUnitRequired && (targetUnit == null || targetUnit.Equals(null)))
                throw new ArgumentException("Target unit cannot be null");
        }

        // Centralized arithmetic logic
        private static double PerformBaseArithmetic(Quantity<U> a, Quantity<U> b, ArithmeticOperation op)
        {
            double aInBase = ConvertToBase(a._value, a._unit);
            double bInBase = ConvertToBase(b._value, b._unit);
            switch (op)
            {
                case ArithmeticOperation.Add:
                    return aInBase + bInBase;
                case ArithmeticOperation.Subtract:
                    return aInBase - bInBase;
                case ArithmeticOperation.Divide:
                    if (Math.Abs(bInBase) < EPSILON)
                        throw new ArithmeticException("Division by zero");
                    return aInBase / bInBase;
                default:
                    throw new InvalidOperationException("Unsupported arithmetic operation");
            }
        }

        // Addition
        public static Quantity<U> Add(Quantity<U> a, Quantity<U> b)
        {
            return Add(a, b, a._unit);
        }

        public static Quantity<U> Add(Quantity<U> a, Quantity<U> b, U targetUnit)
        {
            ValidateArithmeticOperands(a, b, targetUnit, true, ArithmeticOperation.Add);
            // Check if operation is supported (for temperature, etc.)
            ValidateOperationSupportForUnit(a._unit, "add");
            double resultInBase = PerformBaseArithmetic(a, b, ArithmeticOperation.Add);
            double resultInTarget = ConvertFromBase(resultInBase, targetUnit);
            resultInTarget = Math.Round(resultInTarget, 2);
            return new Quantity<U>(resultInTarget, targetUnit);
        }

        // Subtraction
        public static Quantity<U> Subtract(Quantity<U> a, Quantity<U> b)
        {
            return Subtract(a, b, a._unit);
        }

        public static Quantity<U> Subtract(Quantity<U> a, Quantity<U> b, U targetUnit)
        {
            ValidateArithmeticOperands(a, b, targetUnit, true, ArithmeticOperation.Subtract);
            ValidateOperationSupportForUnit(a._unit, "subtract");
            double resultInBase = PerformBaseArithmetic(a, b, ArithmeticOperation.Subtract);
            double resultInTarget = ConvertFromBase(resultInBase, targetUnit);
            resultInTarget = Math.Round(resultInTarget, 2);
            return new Quantity<U>(resultInTarget, targetUnit);
        }

        // Division
        public static double Divide(Quantity<U> a, Quantity<U> b)
        {
            ValidateArithmeticOperands(a, b, null, false, ArithmeticOperation.Divide);
            ValidateOperationSupportForUnit(a._unit, "divide");
            return PerformBaseArithmetic(a, b, ArithmeticOperation.Divide);
        }

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lengthUnit)
                return LengthUnitExtensions.ConvertToBaseUnit(lengthUnit, value);
            if (unit is WeightUnit weightUnit)
                return WeightUnitExtensions.ConvertToBaseUnit(weightUnit, value);
            if (unit is VolumeUnit volumeUnit)
                return VolumeUnitExtensions.ConvertToBaseUnit(volumeUnit, value);
            if (unit is TemperatureUnit tempUnit)
                return TemperatureUnitExtensions.ConvertToBaseUnit(tempUnit, value);
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
            if (unit is TemperatureUnit tempUnit)
                return TemperatureUnitExtensions.ConvertFromBaseUnit(tempUnit, baseValue);
            throw new ArgumentException("Unsupported unit type");
        }

        // Helper to call ValidateOperationSupport if available
        private static void ValidateOperationSupportForUnit(U unit, string operation)
        {
            if (unit is TemperatureUnit tempUnit)
            {
                TemperatureUnitExtensions.ValidateOperationSupport(tempUnit, operation);
            }
            // For other units, do nothing (all operations supported by default)
        }

        public override string ToString()
        {
            return $"Quantity({_value}, {_unit})";
        }
    }
}
