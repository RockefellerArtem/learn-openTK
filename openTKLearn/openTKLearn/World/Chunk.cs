using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using openTKLearn.Graphics;

namespace openTKLearn.World;

public class Chunk
{
    public List<Vector3> _chunkVerts;
    public List<Vector2> _chunkUVs;
    public List<uint> _chunkIndices;

    private const int SIZE = 16;
    private const int HEIGHT = 32;
    public Vector3 _position;

    public uint _indexCount;

    private VAO _chunkVAO;
    private VBO _chunkVertexVBO;
    private VBO _chunkUVVBO;
    private IBO _chunkIBO;
    
    private Texture _texture;
    
    public Chunk(Vector3 postition)
    {
        _position = postition;

        _chunkVerts = new List<Vector3>();
        _chunkUVs = new List<Vector2>();
        _chunkIndices = new List<uint>();

        GenBlocks();
        BuildChunk();
    }

    public void GenChunk()
    {
        
    }
    
    public void GenBlocks() { 
        for(int i = 0; i < 3; i++)
        {
            var block = new Block(new Vector3(i, 0, 0));

            var faceCount = 0;

            if(i == 0)
            {
                var leftFaceData = block.GetFace(BlockData.Faces.LEFT);
                _chunkVerts.AddRange(leftFaceData.vertices);
                _chunkUVs.AddRange(leftFaceData.uv);
                faceCount++;
            }
            
            if (i == 2)
            {
                var rightFaceData = block.GetFace(BlockData.Faces.RIGHT);
                _chunkVerts.AddRange(rightFaceData.vertices);
                _chunkUVs.AddRange(rightFaceData.uv);
                faceCount++;
            }

            var frontFaceData = block.GetFace(BlockData.Faces.FRONT);
            _chunkVerts.AddRange(frontFaceData.vertices);
            _chunkUVs.AddRange(frontFaceData.uv);

            var backFaceData = block.GetFace(BlockData.Faces.BACK);
            _chunkVerts.AddRange(backFaceData.vertices);
            _chunkUVs.AddRange(backFaceData.uv);

            var topFaceData = block.GetFace(BlockData.Faces.TOP);
            _chunkVerts.AddRange(topFaceData.vertices);
            _chunkUVs.AddRange(topFaceData.uv);

            var bottomFaceData = block.GetFace(BlockData.Faces.BOTTOM);
            _chunkVerts.AddRange(bottomFaceData.vertices);
            _chunkUVs.AddRange(bottomFaceData.uv);

            faceCount += 4;

            AddIndices(faceCount);
        }
    }
    
    public void AddIndices(int amtFaces)
    {
        for(int i = 0; i < amtFaces; i++)
        {
            _chunkIndices.Add(0 + _indexCount);
            _chunkIndices.Add(1 + _indexCount);
            _chunkIndices.Add(2 + _indexCount);
            _chunkIndices.Add(2 + _indexCount);
            _chunkIndices.Add(3 + _indexCount);
            _chunkIndices.Add(0 + _indexCount);

            _indexCount += 4;
        }
    }
    
    public void BuildChunk() 
    {
        _chunkVAO = new VAO();
        _chunkVAO.Bind();

        _chunkVertexVBO = new VBO(_chunkVerts);
        _chunkVertexVBO.Bind();
        _chunkVAO.LinkToVAO(0, 3, _chunkVertexVBO);

        _chunkUVVBO = new VBO(_chunkUVs);
        _chunkUVVBO.Bind();
        _chunkVAO.LinkToVAO(1, 2, _chunkUVVBO);

        _chunkIBO = new IBO(_chunkIndices);

        _texture = new Texture("dirtTex.PNG");
    }
    
    public void Render(ShaderProgram program)
    {
        program.Bind();
        _chunkVAO.Bind();
        _chunkIBO.Bind();
        _texture.Bind();
        GL.DrawElements(PrimitiveType.Triangles, _chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
    }

    public void Delete()
    {
        _chunkVAO.Delete();
        _chunkVertexVBO.Delete();
        _chunkUVVBO.Delete();
        _chunkIBO.Delete();
        _texture.Delete();
    }
}