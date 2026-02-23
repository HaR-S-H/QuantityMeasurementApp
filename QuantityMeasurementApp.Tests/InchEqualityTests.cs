using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
/// <summary>
/// Unit tests for the Inches class in the Quantity Measurement application.
/// These tests validate the equality logic implemented in the Inches class, ensuring that measurements are compared accurately
///    and that null values are handled correctly.
/// </summary>
    [TestFixture]
    public class InchesTests
    {  // Test for equality of two Inches instances with the same value
        [Test]
        public void testEquality_SameValue()
        { // Create two Inches instances with the same value
            var i1 = new Inches(1.0);
            var i2 = new Inches(1.0);
             // Assert that the two instances are considered equal based on their values
            Assert.That(i1.Equals(i2), Is.True);
        }
            // Test for equality of two Inches instances with different values
        [Test]
        public void testEquality_DifferentValue()
        {  // Create two Inches instances with different values
            var i1 = new Inches(1.0);
            var i2 = new Inches(2.0);

            Assert.That(i1.Equals(i2), Is.False);
        }
        // Test for equality of an Inches instance compared to null
        [Test]
        public void testEquality_NullComparison()
        {   // Create an Inches instance
            var i1 = new Inches(1.0);
             // Assert that the Inches instance is not considered equal to null
            Assert.That(i1.Equals(null), Is.False);
        }
        // Test for equality of an Inches instance compared to itself (same reference)
        [Test]
        public void testEquality_SameReference()
        {   // Create an Inches instance
            var i1 = new Inches(1.0);
                // Assert that the Inches instance is considered equal to itself based on reference equality
            Assert.That(i1.Equals(i1), Is.True);
        }
    }
}