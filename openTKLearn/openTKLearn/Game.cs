using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace openTKLearn;

public class Game : GameWindow
{
    private float[] _vertices =
    {
        0f, 0.5f, 0f,
        -0.5f, -0.5f, 0f,
        0.5f, -0.5f, 0f
    };

    private int _vao;
    private int _shaderProgram;
    private int _vbo;

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

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length*sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(_vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(_vao, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

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

    }
    
    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteProgram(_shaderProgram);
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(0.6f, 0.3f, 1f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        
        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }
    
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
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
