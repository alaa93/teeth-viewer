using System;
using NUnit.Framework;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class ToothIdTests
    {
        [TestCase(1, 1, 11)]
        [TestCase(2, 6, 26)]
        [TestCase(3, 8, 38)]
        [TestCase(4, 4, 44)]
        public void Fdi_EncodesQuadrantAndPosition(int quadrant, int position, int expectedFdi)
        {
            var id = new ToothId(quadrant, position);
            Assert.AreEqual(expectedFdi, id.Fdi);
        }

        [TestCase(11, true, false)]
        [TestCase(26, true, false)]
        [TestCase(31, false, true)]
        [TestCase(44, false, true)]
        public void FromFdi_RoundTripsAndClassifiesArch(int fdi, bool expectedUpper, bool expectedLower)
        {
            var id = ToothId.FromFdi(fdi);
            Assert.AreEqual(fdi, id.Fdi);
            Assert.AreEqual(expectedUpper, id.IsUpper);
            Assert.AreEqual(expectedLower, id.IsLower);
        }

        [TestCase(0, 1)]
        [TestCase(5, 1)]
        [TestCase(1, 0)]
        [TestCase(1, 9)]
        public void Constructor_RejectsOutOfRangeComponents(int quadrant, int position)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ToothId(quadrant, position));
        }

        [Test]
        public void Equality_IsValueBased()
        {
            var a = new ToothId(1, 1);
            var b = ToothId.FromFdi(11);
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
