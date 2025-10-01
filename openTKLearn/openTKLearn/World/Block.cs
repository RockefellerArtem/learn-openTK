using OpenTK.Mathematics;

namespace openTKLearn.World;

public class Block
{
    public Vector3 position;

    public Dictionary<BlockData.Faces, BlockData.FaceData> faces;

    public List<Vector2> dirtUV = new List<Vector2>
    {
        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),
    };
    
    public Block(Vector3 position) 
    { 
        this.position = position;

        faces = new Dictionary<BlockData.Faces, BlockData.FaceData>
        {
            {BlockData.Faces.FRONT, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.FRONT]),
                uv = dirtUV
            } },
            {BlockData.Faces.BACK, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.BACK]),
                uv = dirtUV
            } },
            {BlockData.Faces.LEFT, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.LEFT]),
                uv = dirtUV
            } },
            {BlockData.Faces.RIGHT, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.RIGHT]),
                uv = dirtUV
            } },
            {BlockData.Faces.TOP, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.TOP]),
                uv = dirtUV
            } },
            {BlockData.Faces.BOTTOM, new BlockData.FaceData {
                vertices = AddTransformedVertices(BlockData.FaceDataRaw.rawVertexData[BlockData.Faces.BOTTOM]),
                uv = dirtUV
            } },

        };
    }
    public List<Vector3> AddTransformedVertices(List<Vector3> vertices) 
    {
        List<Vector3> transformedVertices = new List<Vector3>();
        
        foreach (var vert in vertices) {
            transformedVertices.Add(vert + position);
        }
        
        return transformedVertices;
    }
    
    public BlockData.FaceData GetFace(BlockData.Faces face) 
    {
        return faces[face];
    }
}