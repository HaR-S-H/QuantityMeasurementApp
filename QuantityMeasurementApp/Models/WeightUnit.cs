namespace QuantityMeasurementApp.Models
{
        public enum WeightUnit : int
        {
            Kilogram,
            Gram,
            Pound
        }

        public static class WeightUnitExtensions
        {
            private const double GRAMS_PER_KILOGRAM = 1000.0;
            private const double KILOGRAMS_PER_POUND = 0.453592;

            public static double GetConversionFactor(this WeightUnit unit)
            {
                return unit switch
                {
                    WeightUnit.Kilogram => 1.0,
                    WeightUnit.Gram => 1.0 / GRAMS_PER_KILOGRAM,
                    WeightUnit.Pound => KILOGRAMS_PER_POUND,
                    _ => throw new ArgumentException("Unsupported unit")
                };
            }

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

            public static string GetUnitName(this WeightUnit unit)
            {
                return unit.ToString().ToUpper();
            }
        }
}
