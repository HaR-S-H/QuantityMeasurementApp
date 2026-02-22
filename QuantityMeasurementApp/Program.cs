using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
      
         bool feetResult = QuantityMeasurementService.AreFeetEqual(1.0, 1.0);
        Console.WriteLine($"Feet Equal: {feetResult}");

        bool inchResult = QuantityMeasurementService.AreInchesEqual(1.0, 1.0);
        Console.WriteLine($"Inches Equal: {inchResult}");
    }
    }
