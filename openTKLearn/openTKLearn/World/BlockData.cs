using OpenTK.Mathematics;

namespace openTKLearn.World;

public class BlockData
{
    public enum Faces
    {
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    public struct FaceData
    {
        public List<Vector3> vertices;
        public List<Vector2> uv;
    }

    public struct FaceDataRaw
    {
        public static readonly Dictionary<Faces, List<Vector3>> rawVertexData = new Dictionary<Faces, List<Vector3>>
        {
            {Faces.FRONT, new List<Vector3>()
            {
                new (-0.5f, 0.5f, 0.5f),
                new (0.5f, 0.5f, 0.5f),
                new (0.5f, -0.5f, 0.5f),
                new (-0.5f, -0.5f, 0.5f)
            } },
            {Faces.BACK, new List<Vector3>()
            {
                new (0.5f, 0.5f, -0.5f),
                new (-0.5f, 0.5f, -0.5f),
                new (-0.5f, -0.5f, -0.5f),
                new (0.5f, -0.5f, -0.5f)
            } },
            {Faces.LEFT, new List<Vector3>()
            {
                new (-0.5f, 0.5f, -0.5f),
                new (-0.5f, 0.5f, 0.5f),
                new (-0.5f, -0.5f, 0.5f),
                new (-0.5f, -0.5f, -0.5f)
            } },
            {Faces.RIGHT, new List<Vector3>()
            {
                new (0.5f, 0.5f, 0.5f),
                new (0.5f, 0.5f, -0.5f),
                new (0.5f, -0.5f, -0.5f),
                new (0.5f, -0.5f, 0.5f),
            } },
            {Faces.TOP, new List<Vector3>()
            {
                new (-0.5f, 0.5f, -0.5f),
                new (0.5f, 0.5f, -0.5f),
                new (0.5f, 0.5f, 0.5f),
                new (-0.5f, 0.5f, 0.5f)
            } },
            {Faces.BOTTOM, new List<Vector3>()
            {
                new (-0.5f, -0.5f, 0.5f),
                new (0.5f, -0.5f, 0.5f),
                new (0.5f, -0.5f, -0.5f),
                new (-0.5f, -0.5f, -0.5f)
            } },
        };
    }
}