using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityWeightTests
    {
        [Test]
        public void testEquality_KilogramToKilogram_SameValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var q2 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_KilogramToGram_EquivalentValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var q2 = new QuantityWeight(1000.0, WeightUnit.Gram);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_KilogramToPound_EquivalentValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var q2 = new QuantityWeight(2.20462262, WeightUnit.Pound); // 1 kg = 2.20462262 lb
            Assert.That(q1, Is.EqualTo(q2).Using(new QuantityWeightComparer(1e-5)));
        }

        [Test]
        public void testConversion_KilogramToGram()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var result = q1.ConvertTo(WeightUnit.Gram);
            Assert.That(result, Is.EqualTo(new QuantityWeight(1000.0, WeightUnit.Gram)).Using(new QuantityWeightComparer(1e-4)));
        }

        [Test]
        public void testAddition_KilogramAndGram_DefaultUnit()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var q2 = new QuantityWeight(1000.0, WeightUnit.Gram);
            var result = QuantityWeight.Add(q1, q2);
            Assert.That(result, Is.EqualTo(new QuantityWeight(2.0, WeightUnit.Kilogram)).Using(new QuantityWeightComparer(1e-4)));
        }

        [Test]
        public void testAddition_KilogramAndGram_ExplicitGram()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var q2 = new QuantityWeight(1000.0, WeightUnit.Gram);
            var result = QuantityWeight.Add(q1, q2, WeightUnit.Gram);
            Assert.That(result, Is.EqualTo(new QuantityWeight(2000.0, WeightUnit.Gram)).Using(new QuantityWeightComparer(1e-4)));
        }

        // Helper for approximate equality
        private class QuantityWeightComparer : IEqualityComparer<QuantityWeight>
        {
            private readonly double _epsilon;
            public QuantityWeightComparer(double epsilon) { _epsilon = epsilon; }
            public bool Equals(QuantityWeight x, QuantityWeight y)
            {
                if (x == null || y == null) return false;
                double xInYUnit = x.ConvertTo(y.GetUnit()).GetValue();
                return Math.Abs(xInYUnit - y.GetValue()) < _epsilon;
            }
            public int GetHashCode(QuantityWeight obj) => obj.GetHashCode();
        }
    }
}
