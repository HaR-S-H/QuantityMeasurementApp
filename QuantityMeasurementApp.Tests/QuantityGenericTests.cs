using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityGenericTests
    {
        [Test]
        public void testGenericQuantity_LengthEquality()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testGenericQuantity_WeightEquality()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testGenericQuantity_LengthConversion()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var result = q1.ConvertTo(LengthUnit.Inch);
            Assert.That(result.GetValue(), Is.EqualTo(12.0).Within(1e-4));
        }

        [Test]
        public void testGenericQuantity_WeightConversion()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var result = q1.ConvertTo(WeightUnit.Gram);
            Assert.That(result.GetValue(), Is.EqualTo(1000.0).Within(1e-4));
        }

        [Test]
        public void testGenericQuantity_LengthAddition()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            var result = Quantity<LengthUnit>.Add(q1, q2, LengthUnit.Feet);
            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-4));
        }

        [Test]
        public void testGenericQuantity_WeightAddition()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);
            var result = Quantity<WeightUnit>.Add(q1, q2, WeightUnit.Kilogram);
            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-4));
        }

        [Test]
        public void testGenericQuantity_CrossCategoryEquality()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            Assert.That(q1.Equals(q2), Is.False);
        }
    }
}
