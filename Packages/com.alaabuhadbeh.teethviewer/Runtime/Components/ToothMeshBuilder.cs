using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public enum ToothArchetype
    {
        Incisor,
        Canine,
        Premolar,
        Molar
    }

    public static class ToothMeshBuilder
    {
        public static Mesh Build(ToothArchetype archetype, int radialSegments = 20, int heightSegments = 14)
        {
            GetDimensions(archetype, out float height, out float radius, out float depthScale, out float tipSharpness);

            int ringVerts = radialSegments + 1;
            int vertCount = (heightSegments + 1) * ringVerts + 1;
            var vertices = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            for (int ring = 0; ring <= heightSegments; ring++)
            {
                float v = (float)ring / heightSegments;
                float r = radius * Profile(v, tipSharpness);
                float y = v * height;
                for (int s = 0; s < ringVerts; s++)
                {
                    float u = (float)s / radialSegments;
                    float angle = u * Mathf.PI * 2f;
                    int i = ring * ringVerts + s;

                    vertices[i] = new Vector3(Mathf.Cos(angle) * r, y, Mathf.Sin(angle) * r * depthScale);
                    uvs[i] = new Vector2(u, v);
                }
            }

            int baseCentre = vertCount - 1;
            vertices[baseCentre] = Vector3.zero;
            uvs[baseCentre] = new Vector2(0.5f, 0f);

            var triangles = new int[heightSegments * radialSegments * 6 + radialSegments * 3];
            int t = 0;
            for (int ring = 0; ring < heightSegments; ring++)
            {
                for (int s = 0; s < radialSegments; s++)
                {
                    int a = ring * ringVerts + s;
                    int b = a + 1;
                    int c = a + ringVerts;
                    int d = c + 1;
                    triangles[t++] = a; triangles[t++] = d; triangles[t++] = b;
                    triangles[t++] = a; triangles[t++] = c; triangles[t++] = d;
                }
            }
            for (int s = 0; s < radialSegments; s++)
            {
                triangles[t++] = baseCentre;
                triangles[t++] = s;
                triangles[t++] = s + 1;
            }

            var mesh = new Mesh { name = $"Tooth_{archetype}" };
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static float Profile(float v, float tipSharpness)
        {
            float rise = Mathf.SmoothStep(0.66f, 1f, Mathf.Clamp01(v / 0.45f));
            if (v <= 0.55f)
                return rise;
            float f = (v - 0.55f) / 0.45f;

            float taper = Mathf.Max(0f, Mathf.Cos(f * Mathf.PI * 0.5f));
            return rise * Mathf.Pow(taper, tipSharpness);
        }

        private static void GetDimensions(
            ToothArchetype archetype,
            out float height,
            out float radius,
            out float depthScale,
            out float tipSharpness)
        {
            switch (archetype)
            {
                case ToothArchetype.Incisor:
                    height = 0.95f; radius = 0.30f; depthScale = 0.45f; tipSharpness = 1.1f;
                    break;
                case ToothArchetype.Canine:
                    height = 1.05f; radius = 0.32f; depthScale = 0.62f; tipSharpness = 1.5f;
                    break;
                case ToothArchetype.Premolar:
                    height = 0.90f; radius = 0.36f; depthScale = 0.85f; tipSharpness = 0.9f;
                    break;
                default:
                    height = 0.85f; radius = 0.44f; depthScale = 0.95f; tipSharpness = 0.65f;
                    break;
            }
        }
    }
}
