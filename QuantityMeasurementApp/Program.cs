using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
        var service = new QuantityMeasurementService();

        Feet f1 = new Feet(1.0);
        Feet f2 = new Feet(1.0);

        bool result = service.AreEqual(f1, f2);

        Console.WriteLine($"Equal: {result}");
    }
}