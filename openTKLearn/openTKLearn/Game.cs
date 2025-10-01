using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace openTKLearn;

public class Game : GameWindow
{
    private List<Vector3> _vertices = new List<Vector3>()
    {
        new (-0.5f, 0.5f, 0.5f),
        new (0.5f, 0.5f, 0.5f),
        new (0.5f, -0.5f, 0.5f),
        new (-0.5f, -0.5f, 0.5f),
        new (0.5f, 0.5f, 0.5f),
        new (0.5f, 0.5f, -0.5f),
        new (0.5f, -0.5f, -0.5f),
        new (0.5f, -0.5f, 0.5f),
        new (0.5f, 0.5f, -0.5f),
        new (-0.5f, 0.5f, -0.5f),
        new (-0.5f, -0.5f, -0.5f),
        new (0.5f, -0.5f, -0.5f),
        new (-0.5f, 0.5f, -0.5f),
        new (-0.5f, 0.5f, 0.5f),
        new (-0.5f, -0.5f, 0.5f),
        new (-0.5f, -0.5f, -0.5f),
        new (-0.5f, 0.5f, -0.5f),
        new (0.5f, 0.5f, -0.5f),
        new (0.5f, 0.5f, 0.5f),
        new (-0.5f, 0.5f, 0.5f),
        new (-0.5f, -0.5f, 0.5f),
        new (0.5f, -0.5f, 0.5f),
        new (0.5f, -0.5f, -0.5f), 
        new (-0.5f, -0.5f, -0.5f)
    };

    private List<Vector2> _texCoords = new List<Vector2>()
    {
        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),

        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),

        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),

        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),

        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),

        new (0f, 1f),
        new (1f, 1f),
        new (1f, 0f),
        new (0f, 0f),
    };

    private uint[] _indices =
    {
        0, 1, 2,
        2, 3, 0,

        4, 5, 6,
        6, 7, 4,

        8, 9, 10,
        10, 11, 8,

        12, 13, 14,
        14, 15, 12,

        16, 17, 18,
        18, 19, 16,

        20, 21, 22,
        22, 23, 20
    };

    private int _vao;
    private int _shaderProgram;
    private int _vbo;
    private int _textureVBO;
    private int _ebo;
    private int _textureID;

    private Camera _camera;

    private float _yRot = 0f;

    private int _width;
    private int _height;
    
    public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        _width = width;
        _height = height;

        CenterWindow(new Vector2i(width, height));
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0,0, e.Width, e.Height);
        
        _width = e.Width;
        _height = e.Height;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        _vao = GL.GenVertexArray();

        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes, _vertices.ToArray(), BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(_vao, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _textureVBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _textureVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, _texCoords.Count * Vector2.SizeInBytes, _texCoords.ToArray(), BufferUsageHint.StaticDraw);
        

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(_vao, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length*sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        _shaderProgram = GL.CreateProgram();

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert")); 
        GL.CompileShader(vertexShader);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
        GL.CompileShader(fragmentShader);

        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);

        GL.LinkProgram(_shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        _textureID = GL.GenTexture();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _textureID);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        StbImage.stbi_set_flip_vertically_on_load(1);
        
        var dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/dirtTex.PNG"), ColorComponents.RedGreenBlueAlpha);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.Enable(EnableCap.DepthTest);

        _camera = new Camera(_width, _height, Vector3.Zero);
        CursorState = CursorState.Grabbed;
    }
    
    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteTexture(_textureID);
        GL.DeleteProgram(_shaderProgram);
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(0.3f, 0.3f, 1f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        GL.BindTexture(TextureTarget.Texture2D, _textureID);
        
        var model = Matrix4.Identity;
        var view = _camera.GetViewMatrix();
        var projection = _camera.GetProjectionMatrix();

        
        model = Matrix4.CreateRotationY(_yRot);
        _yRot += 0.001f;

        Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

        model *= translation;

        var modelLocation = GL.GetUniformLocation(_shaderProgram, "model");
        var viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
        var projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionLocation, true, ref projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
    
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        var mouse = MouseState;
        var input = KeyboardState;

        base.OnUpdateFrame(args);
        _camera.Update(input, mouse, args);
    }

    public static string LoadShaderSource(string filePath)
    {
        var shaderSource = "";

        try
        {
            using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
            {
                shaderSource = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to load shader source file: " + e.Message);
        }

        return shaderSource;
    }

}
