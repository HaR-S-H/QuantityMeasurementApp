using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityVolumeTests
    {
        [Test]
        public void testEquality_LitreToLitre_SameValue()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_LitreToMillilitre_EquivalentValue()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_LitreToGallon_EquivalentValue()
        {
            var q1 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testConversion_LitreToMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var result = q1.ConvertTo(VolumeUnit.Millilitre);
            Assert.That(result.GetValue(), Is.EqualTo(1000.0).Within(1e-4));
        }

        [Test]
        public void testAddition_LitreAndMillilitre_DefaultUnit()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            var result = Quantity<VolumeUnit>.Add(q1, q2);
            Assert.That(result.GetValue(), Is.EqualTo(2.0).Within(1e-4));
        }

        [Test]
        public void testAddition_LitreAndGallon_ExplicitMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            var result = Quantity<VolumeUnit>.Add(q1, q2, VolumeUnit.Millilitre);
            Assert.That(result.GetValue(), Is.EqualTo(4785.41).Within(1e-2));
        }
    }
}
