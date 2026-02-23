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
    public class YardEqualityTests
    {
        [Test]
        public void testEquality_YardToFeet()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var feet = new QuantityLength(3, LengthUnit.Feet);

            Assert.That(yard.Equals(feet), Is.True);
        }

        [Test]
        public void testEquality_YardToInches()
        {
            var yard = new QuantityLength(1, LengthUnit.Yard);
            var inch = new QuantityLength(36, LengthUnit.Inch);

            Assert.That(yard.Equals(inch), Is.True);
        }
        [Test]
public void testEquality_CmToInch()
{
    var cm = new QuantityLength(1, LengthUnit.Centimeter);
    var inch = new QuantityLength(0.393701, LengthUnit.Inch);

    Assert.That(cm.Equals(inch), Is.True);
}   
[Test]
public void testEquality_CmToFeet_NotEqual()
{
    var cm = new QuantityLength(1, LengthUnit.Centimeter);
    var feet = new QuantityLength(1, LengthUnit.Feet);

    Assert.That(cm.Equals(feet), Is.False);
}
    }
}