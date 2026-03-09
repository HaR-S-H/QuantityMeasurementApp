namespace QuantityMeasurementApp.Models
{
    public enum VolumeUnit
    {
        Litre,
        Millilitre,
        Gallon
    }

    public static class VolumeUnitExtensions
    {
        private const double MILLILITRES_PER_LITRE = 1000.0;
        private const double LITRES_PER_GALLON = 3.78541;

        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => 1.0,
                VolumeUnit.Millilitre => 1.0 / MILLILITRES_PER_LITRE,
                VolumeUnit.Gallon => LITRES_PER_GALLON,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return unit switch
            {
                VolumeUnit.Litre => value,
                VolumeUnit.Millilitre => value / MILLILITRES_PER_LITRE,
                VolumeUnit.Gallon => value * LITRES_PER_GALLON,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return unit switch
            {
                VolumeUnit.Litre => baseValue,
                VolumeUnit.Millilitre => baseValue * MILLILITRES_PER_LITRE,
                VolumeUnit.Gallon => baseValue / LITRES_PER_GALLON,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}
