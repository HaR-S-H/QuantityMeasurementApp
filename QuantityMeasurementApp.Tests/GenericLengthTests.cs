using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{ /// <summary>
  /// Unit tests for the QuantityLength class in the Quantity Measurement application.
  /// These tests validate the equality logic implemented in the QuantityLength class, ensuring that measurements are compared accurately
  ///     and that null values are handled correctly.
  /// </summary>
    public class GenericLengthTests
    { // Test for equality of two QuantityLength instances with the same value and unit (Feet)
        [Test]
        public void testEquality_FeetToFeet_SameValue()
        {   // Create two QuantityLength instances with the same value and unit (Feet)
            var q1 = new QuantityLength(1, LengthUnit.Feet);
            var q2 = new QuantityLength(1, LengthUnit.Feet);
             // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
        // Test for equality of two QuantityLength instances with the same value and unit (Inch)
        [Test]
        public void testEquality_InchToInch_SameValue()
        { // Create two QuantityLength instances with the same value and unit (Inch)
            var q1 = new QuantityLength(1, LengthUnit.Inch);
            var q2 = new QuantityLength(1, LengthUnit.Inch);
                // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
      // Test for equality of two QuantityLength instances with different values and units
        [Test]
        public void testEquality_InchToFeet_EquivalentValue()
        {  // Create two QuantityLength instances with different values and units (Inch and Feet) that are equivalent
            var q1 = new QuantityLength(12, LengthUnit.Inch);
            var q2 = new QuantityLength(1, LengthUnit.Feet);
            // Assert that the two instances are considered equal based on their values and units
            Assert.That(q1.Equals(q2), Is.True);
        }
        [Test]
        // Test for equality of two QuantityLength instances with different values and the same unit (Feet)
        public void testEquality_FeetToFeet_DifferentValue()
        { // Create two QuantityLength instances with different values and the same unit (Feet)
            var q1 = new QuantityLength(1, LengthUnit.Feet);
            var q2 = new QuantityLength(2, LengthUnit.Feet);
            // Assert that the two instances are not considered equal based on their values
            Assert.That(q1.Equals(q2), Is.False);
        }
         // Test for equality of a QuantityLength instance with a null value
        [Test]
        public void testEquality_NullComparison()
        {   // Create a QuantityLength instance
            var q1 = new QuantityLength(1, LengthUnit.Feet);
              // Assert that the QuantityLength instance is not considered equal to null
            Assert.That(q1.Equals(null), Is.False);
        }
       // Test for equality of a QuantityLength instance compared to itself (same reference)
        [Test]
        public void testEquality_SameReference()
        {   // Create a QuantityLength instance
            var q1 = new QuantityLength(1, LengthUnit.Feet);
             // Assert that the QuantityLength instance is considered equal to itself based on reference equality
            Assert.That(q1.Equals(q1), Is.True);
        }
    }
}