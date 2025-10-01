using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace openTKLearn;

public class Camera
{
    private float _speed = 8f;
    private float _screenWidth;
    private float _screenHeight;
    private float _sensitivity = 180f;

    public Vector3 _position;

    private Vector3 up = Vector3.UnitY;
    private Vector3 front = -Vector3.UnitZ;
    private Vector3 right = Vector3.UnitX;

    private float pitch;
    private float yaw = -90.0f;

    private bool firstMove = true;
    
    public Vector2 lastPos;
    
    public Camera(float width, float height, Vector3 position) 
    {
        _screenWidth = width;
        _screenHeight = height;
        _position = position;
    }

    public Matrix4 GetViewMatrix() 
    {
        return Matrix4.LookAt(_position, _position + front, up);
    }
    
    public Matrix4 GetProjectionMatrix() 
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _screenWidth / _screenHeight, 0.1f, 100.0f);
    }

    private void UpdateVectors()
    {
        if (pitch > 89.0f)
        {
            pitch = 89.0f;
        }
        if (pitch < -89.0f)
        {
            pitch = -89.0f;
        }


        front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
        front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
        front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

        front = Vector3.Normalize(front);

        right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        up = Vector3.Normalize(Vector3.Cross(right, front));
    }

    public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) 
    {
    
        if (input.IsKeyDown(Keys.W))
        {
            _position += front * _speed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.A))
        {
            _position -= right * _speed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.S))
        {
            _position -= front * _speed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.D))
        {
            _position += right * _speed * (float)e.Time;
        }

        if (input.IsKeyDown(Keys.Space))
        {
            _position.Y += _speed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _position.Y -= _speed * (float)e.Time;
        }

        if (firstMove)
        {
            lastPos = new Vector2(mouse.X, mouse.Y);
            firstMove = false;
        } 
        
        else
        {
            var deltaX = mouse.X - lastPos.X;
            var deltaY = mouse.Y - lastPos.Y;
            lastPos = new Vector2(mouse.X, mouse.Y);

            yaw += deltaX * _sensitivity * (float)e.Time;
            pitch -= deltaY * _sensitivity * (float)e.Time;
        }
        
        UpdateVectors();
    }
    
    public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) 
    {
        InputController(input, mouse, e);
    }
}