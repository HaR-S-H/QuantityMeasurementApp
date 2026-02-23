using NUnit.Framework;
using QuantityMeasurementApp.Models;
namespace QuantityMeasurementApp.Tests
{
    
/// <summary>
/// Unit tests for the Feet class in the Quantity Measurement application.
/// These tests validate the equality logic implemented in the Feet class, ensuring that measurements are compared accurately
///     and that null values are handled correctly.
/// </summary>
    public class FeetTests
    {// Test for equality of two Feet instances with the same value
        [Test]
        public void testEquality_SameValue()
        {
            // Create two Feet instances with the same value
            var f1 = new Feet(1.0);
            var f2 = new Feet(1.0);
            // Assert that the two instances are considered equal based on their values
            Assert.That(f1.Equals(f2), Is.True);
        }
        // Test for equality of two Feet instances with different values
        [Test]
        public void testEquality_DifferentValue()
        {   // Create two Feet instances with different values
            var f1 = new Feet(1.0);
            var f2 = new Feet(2.0);
            // Assert that the two instances are not considered equal based on their values
            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        // Test for equality of a Feet instance compared to null
        public void testEquality_NullComparison()
        {
            // Create a Feet instance
            var f1 = new Feet(1.0);
            // Assert that the Feet instance is not considered equal to null
            Assert.That(f1.Equals(null), Is.False);
        }
        // Test for equality of a Feet instance compared to itself (same reference)
        [Test]
        public void testEquality_SameReference()
        {
            // Create a Feet instance
            var f1 = new Feet(1.0);
            // Assert that the Feet instance is considered equal to itself based on reference equality
            Assert.That(f1.Equals(f1), Is.True);
        }
    }
}