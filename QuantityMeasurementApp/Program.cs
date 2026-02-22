using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
           var q1 = new QuantityLength(1.0, LengthUnit.Feet);
        var q2 = new QuantityLength(12.0, LengthUnit.Inch);

        Console.WriteLine(q1.Equals(q2)); // true
    }
    }
