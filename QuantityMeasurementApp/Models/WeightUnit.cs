namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    public static class WeightUnitMethods
    {
        private const double GRAMS_PER_KILOGRAM = 1000.0;
        private const double KILOGRAMS_PER_POUND = 0.453592;

        // Convert a value in this unit to kilograms (base unit)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.Kilogram => value,
                WeightUnit.Gram => value / GRAMS_PER_KILOGRAM,
                WeightUnit.Pound => value * KILOGRAMS_PER_POUND,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // Convert a value in kilograms (base unit) to this unit
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.Kilogram => baseValue,
                WeightUnit.Gram => baseValue * GRAMS_PER_KILOGRAM,
                WeightUnit.Pound => baseValue / KILOGRAMS_PER_POUND,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}
