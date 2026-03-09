using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityLengthUC7Tests
    {
        [Test]
        public void testAddition_ExplicitTargetUnit_Feet()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);
            var result = QuantityLength.Add(q1, q2, LengthUnit.Feet);
            Assert.That(result, Is.EqualTo(new QuantityLength(2.0, LengthUnit.Feet)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Inches()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);
            var result = QuantityLength.Add(q1, q2, LengthUnit.Inch);
            Assert.That(result, Is.EqualTo(new QuantityLength(24.0, LengthUnit.Inch)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Yards()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);
            var result = QuantityLength.Add(q1, q2, LengthUnit.Yard);
            Assert.That(result, Is.EqualTo(new QuantityLength(0.666667, LengthUnit.Yard)).Using(new QuantityLengthComparer(1e-5)));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit_Centimeters()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Inch);
            var q2 = new QuantityLength(1.0, LengthUnit.Inch);
            var result = QuantityLength.Add(q1, q2, LengthUnit.Centimeter);
            Assert.That(result, Is.EqualTo(new QuantityLength(5.08, LengthUnit.Centimeter)).Using(new QuantityLengthComparer(1e-2)));
        }


        // Helper for approximate equality
        private class QuantityLengthComparer : IEqualityComparer<QuantityLength>
        {
            private readonly double _epsilon;
            public QuantityLengthComparer(double epsilon) { _epsilon = epsilon; }
            public bool Equals(QuantityLength x, QuantityLength y)
            {
                if (x == null || y == null) return false;
                // Compare values in the same unit using public API
                double xInYUnit = x.ConvertTo(y.GetUnit());
                return Math.Abs(xInYUnit - y.GetValue()) < _epsilon;
            }
            public int GetHashCode(QuantityLength obj) => obj.GetHashCode();
        }
    }
}
