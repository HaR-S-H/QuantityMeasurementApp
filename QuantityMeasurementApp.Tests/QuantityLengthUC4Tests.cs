using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests for the QuantityLength class in the Quantity Measurement application.
    /// These tests validate the equality logic implemented in the QuantityLength class for various length units.
    /// The tests ensure that measurements are compared accurately across different units (Yard, Feet, Inch, Centimeter) and that the transitive property of equality holds true.
    /// </summary> 
    public class QuantityLengthUC4Tests
    {   // Test for equality of two QuantityLength instances with the same value and unit (Yard)
        [Test]
        public void testEquality_YardToYard_SameValue()
        { // Create two QuantityLength instances with the same value and unit (Yard)
            var q1 = new QuantityLength(1, LengthUnit.Yard);
            var q2 = new QuantityLength(1, LengthUnit.Yard);
            // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
        // Test for equality of two QuantityLength instances with the same value and unit (Feet)
        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {   // Create two QuantityLength instances with different values and units (Yard and Feet) that are equivalent
            var q1 = new QuantityLength(1, LengthUnit.Yard);
            var q2 = new QuantityLength(3, LengthUnit.Feet);
            // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
        // Test for equality of two QuantityLength instances with different values and units (Yard and Inch) that are equivalent
        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {       // Create two QuantityLength instances with different values and units (Yard and Inch) that are equivalent
            var q1 = new QuantityLength(1, LengthUnit.Yard);
            var q2 = new QuantityLength(36, LengthUnit.Inch);
                // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
        // Test for equality of two QuantityLength instances with different values and units (Yard and Centimeter) that are equivalent
        [Test]
        public void testEquality_CentimeterToInch_EquivalentValue()
        {       // Create two QuantityLength instances with different values and units (Centimeter and Inch) that are equivalent
            var q1 = new QuantityLength(1, LengthUnit.Centimeter);
            var q2 = new QuantityLength(0.393701, LengthUnit.Inch);
            // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
            // Test for equality of two QuantityLength instances with different values and the same unit (Feet)
        [Test]
        public void testEquality_AllUnits_TransitiveProperty()
        {   // Create three QuantityLength instances with different values and units (Yard, Feet, Inch) that are equivalent
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);
            var inch = new QuantityLength(36, LengthUnit.Inch);
            // Assert that the transitive property of equality holds true (Yard equals Feet, Feet equals Inch, therefore Yard equals Inch)
            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inch), Is.True);
            Assert.That(yard.Equals(inch), Is.True);
        }
    }
}