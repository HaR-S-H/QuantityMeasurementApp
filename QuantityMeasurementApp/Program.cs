using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
       Console.WriteLine("===== UC5 Unit Conversion Demo =====\n");

            DemonstrateLengthConversion(1.0, LengthUnit.Feet, LengthUnit.Inch);
            DemonstrateLengthConversion(3.0, LengthUnit.Yard, LengthUnit.Feet);
            DemonstrateLengthConversion(36.0, LengthUnit.Inch, LengthUnit.Yard);
            DemonstrateLengthConversion(2.54, LengthUnit.Centimeter, LengthUnit.Inch);
            DemonstrateLengthConversion(0.0, LengthUnit.Feet, LengthUnit.Inch);
        }

        // Method 1 → convert using raw values
        public static void DemonstrateLengthConversion(double value,
                                                       LengthUnit from,
                                                       LengthUnit to)
        {
            double result = QuantityLength.Convert(value, from, to);

            Console.WriteLine($"Input: convert({value}, {from}, {to}) → Output: {result}");
        }
    }
    }
