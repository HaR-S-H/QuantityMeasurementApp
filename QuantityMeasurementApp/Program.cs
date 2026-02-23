using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
              Console.WriteLine("=== UC4: Extended Unit Support ===");

        // Yard → Feet
        var q1 = new QuantityLength(1.0, LengthUnit.Yard);
        var q2 = new QuantityLength(3.0, LengthUnit.Feet);

        Console.WriteLine($"1 Yard == 3 Feet → {q1.Equals(q2)}");


        // Yard → Inches
        var q3 = new QuantityLength(1.0, LengthUnit.Yard);
        var q4 = new QuantityLength(36.0, LengthUnit.Inch);

        Console.WriteLine($"1 Yard == 36 Inches → {q3.Equals(q4)}");


        // Centimeter → Inch
        var q5 = new QuantityLength(1.0, LengthUnit.Centimeter);
        var q6 = new QuantityLength(0.393701, LengthUnit.Inch);

        Console.WriteLine($"1 CM == 0.393701 Inch → {q5.Equals(q6)}");


        // Same unit comparison
        var q7 = new QuantityLength(2.0, LengthUnit.Yard);
        var q8 = new QuantityLength(2.0, LengthUnit.Yard);

        Console.WriteLine($"2 Yard == 2 Yard → {q7.Equals(q8)}");


        // Not equal case
        var q9 = new QuantityLength(1.0, LengthUnit.Centimeter);
        var q10 = new QuantityLength(1.0, LengthUnit.Feet);

        Console.WriteLine($"1 CM == 1 Feet → {q9.Equals(q10)}");
    }
    }
