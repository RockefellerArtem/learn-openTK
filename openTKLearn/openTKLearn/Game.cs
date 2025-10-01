using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using openTKLearn.Graphics;
using openTKLearn.World;
using StbImageSharp;

namespace openTKLearn;

public class Game : GameWindow
{
    private Chunk _chunk;
    
    private ShaderProgram _program;

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

        _chunk = new Chunk(new Vector3(0, 0, 0));
        _program = new ShaderProgram("Default.vert", "Default.frag");

        GL.Enable(EnableCap.DepthTest);

        _camera = new Camera(_width, _height, Vector3.Zero);
        CursorState = CursorState.Grabbed;
    }
    
    protected override void OnUnload()
    {
        base.OnUnload();

        _chunk.Delete();
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(0.3f, 0.3f, 1f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var model = Matrix4.Identity;
        var view = _camera.GetViewMatrix();
        var projection = _camera.GetProjectionMatrix();

        
        var modelLocation = GL.GetUniformLocation(_program.ID, "model");
        var viewLocation = GL.GetUniformLocation(_program.ID, "view");
        var projectionLocation = GL.GetUniformLocation(_program.ID, "projection");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionLocation, true, ref projection);

        _chunk.Render(_program);

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
}
