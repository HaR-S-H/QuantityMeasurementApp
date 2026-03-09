namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable
    {
        double GetConversionFactor();
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);
        string GetUnitName();

        // Default: all units support arithmetic unless overridden
        bool SupportsArithmetic() { return true; }

        // Default: do nothing (all operations supported)
        void ValidateOperationSupport(string operation) { /* no-op */ }
    }
}
