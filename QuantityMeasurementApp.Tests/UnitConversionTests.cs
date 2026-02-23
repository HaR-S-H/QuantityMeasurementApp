using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{   /// <summary>
    ///   Unit tests for the unit conversion functionality in the Quantity Measurement application.
    /// These tests validate the correctness of the conversion logic implemented in the QuantityLength class, ensuring
    /// that measurements are converted accurately between different length units (Feet, Inches, Yards, Centimeters) and that edge cases (zero, negative values, NaN, Infinity) are handled appropriately.
    /// </summary>
    public class UnitConversionTests
    {   // A small epsilon value for comparing floating-point numbers in tests to account for precision issues
        private const double EPSILON = 0.0001;

        // Test for conversion of 1 Foot to Inches
        [Test]
        public void testConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }
    // Test for conversion of 1 Yard to Inches
        [Test]
        public void testConversion_InchesToFeet()
        {
            double result = QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);

            Assert.That(result, Is.EqualTo(2.0).Within(EPSILON));
        }
    // Test for conversion of 2.54 Centimeters to Inches
        [Test]
        public void testConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);

            Assert.That(result, Is.EqualTo(36.0).Within(EPSILON));
        }

        // Test for conversion of 1 Inch to Centimeters
        [Test]
        public void testConversion_InchesToYards()
        {
            double result = QuantityLength.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);

            Assert.That(result, Is.EqualTo(2.0).Within(EPSILON));
        }

     // Test for conversion of 1 Centimeter to Inches
        [Test]
        public void testConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);

            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        // Test for conversion of 1 Inch to Centimeters
        [Test]
        public void testConversion_FeetToYards()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);

            Assert.That(result, Is.EqualTo(2.0).Within(EPSILON));
        }

    // Test for conversion of 1 Yard to Feet
        [Test]
        public void testConversion_SameUnit_ReturnsSameValue()
        {
            double result = QuantityLength.Convert(5.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.That(result, Is.EqualTo(5.0).Within(EPSILON));
        }

        // Test for conversion of 0 Feet to Inches
        [Test]
        public void testConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.That(result, Is.EqualTo(0.0));
        }

        // Test for conversion of a negative value from Feet to Inches
        [Test]
        public void testConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.That(result, Is.EqualTo(-12.0).Within(EPSILON));
        }   

        // Test for conversion of a value from Feet to Inches and back to Feet, ensuring that the original value is preserved within a reasonable tolerance
        [Test]
        public void testConversion_RoundTrip_PreservesValue()
        {
            double original = 5.0;

            double inches = QuantityLength.Convert(original, LengthUnit.Feet, LengthUnit.Inch);
            double backToFeet = QuantityLength.Convert(inches, LengthUnit.Inch, LengthUnit.Feet);

            Assert.That(backToFeet, Is.EqualTo(original).Within(EPSILON));
        }
        // Test for conversion of a value from Yards to Centimeters and back to Yards, ensuring that the original value is preserved within a reasonable tolerance
        [Test]
        public void testConversion_NaN_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch));
        }
        // Test for conversion of a value from Yards to Centimeters and back to Yards, ensuring that the original value is preserved within a reasonable tolerance
        [Test]
        public void testConversion_Infinity_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inch));
        }
    }
}