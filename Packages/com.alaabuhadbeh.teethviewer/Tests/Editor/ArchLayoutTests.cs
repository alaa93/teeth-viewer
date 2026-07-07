using NUnit.Framework;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class ArchLayoutTests
    {
        [Test]
        public void BuildRestStage_Contains32Teeth()
        {
            var stage = ArchLayout.BuildRestStage();
            Assert.AreEqual(32, stage.ToothCount);
        }

        [Test]
        public void BuildRestStage_EveryFdiToothIsPresent()
        {
            var stage = ArchLayout.BuildRestStage();
            for (int quadrant = 1; quadrant <= 4; quadrant++)
                for (int position = 1; position <= 8; position++)
                    Assert.IsTrue(stage.Contains(new ToothId(quadrant, position)),
                        $"Missing tooth {quadrant}{position}");
        }

        [TestCase(1, ToothArchetype.Incisor)]
        [TestCase(2, ToothArchetype.Incisor)]
        [TestCase(3, ToothArchetype.Canine)]
        [TestCase(4, ToothArchetype.Premolar)]
        [TestCase(5, ToothArchetype.Premolar)]
        [TestCase(6, ToothArchetype.Molar)]
        [TestCase(7, ToothArchetype.Molar)]
        [TestCase(8, ToothArchetype.Molar)]
        public void ArchetypeFor_MapsPositionToClinicalToothType(int position, ToothArchetype expected)
        {
            var archetype = ArchLayout.ArchetypeFor(new ToothId(1, position));
            Assert.AreEqual(expected, archetype);
        }
    }
}
