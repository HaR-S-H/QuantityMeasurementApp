using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
      var yard = new QuantityLength(1, LengthUnit.Yard);
        var feet = new QuantityLength(3, LengthUnit.Feet);
        var inches = new QuantityLength(36, LengthUnit.Inch);
        var cm = new QuantityLength(1, LengthUnit.Centimeter);
        var inch = new QuantityLength(0.393701, LengthUnit.Inch);

        Console.WriteLine(yard.Equals(feet));   // true
        Console.WriteLine(yard.Equals(inches)); // true
        Console.WriteLine(cm.Equals(inch));     // true
    }
    }
