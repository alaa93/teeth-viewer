using NUnit.Framework;
using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer.Tests
{
    public class ToothMeshBuilderTests
    {
        [TestCase(ToothArchetype.Incisor)]
        [TestCase(ToothArchetype.Canine)]
        [TestCase(ToothArchetype.Premolar)]
        [TestCase(ToothArchetype.Molar)]
        public void Build_ProducesOnlyFiniteVertices(ToothArchetype archetype)
        {
            var mesh = ToothMeshBuilder.Build(archetype);
            foreach (var v in mesh.vertices)
            {
                Assert.IsFalse(float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z),
                    $"{archetype} mesh has a NaN vertex: {v}");
                Assert.IsFalse(float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z),
                    $"{archetype} mesh has an infinite vertex: {v}");
            }
        }

        [TestCase(ToothArchetype.Incisor)]
        [TestCase(ToothArchetype.Molar)]
        public void Build_ProducesNonEmptyMeshWithBounds(ToothArchetype archetype)
        {
            var mesh = ToothMeshBuilder.Build(archetype);
            Assert.Greater(mesh.vertexCount, 0);
            Assert.Greater(mesh.triangles.Length, 0);
            Assert.Greater(mesh.bounds.size.magnitude, 0f);
        }
    }
}
