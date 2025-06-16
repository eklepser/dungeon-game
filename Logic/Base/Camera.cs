using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Venefica.Logic.Base;

internal class Camera
{
    public Vector2 Position { get; set; }
    private GameObject _target;
    public Rectangle FOV
    {
        get => new(-(int)Position.X, -(int)Position.Y, 1280 * 2, 720 * 2);
    }

    public Camera(Vector2 position, GameObject target)
    {
        Position = position;
        _target = target;
    }
    public Camera(GameObject target)
    {
        Position = new Vector2(0, 0);
        _target = target;
    }

    public void Update(Vector2 screenSize, GameObject target)
    {
        Position = new Vector2(
            -target.RectDst.X + screenSize.X / 2 - target.RectDst.Width / 2,
            -target.RectDst.Y + screenSize.Y / 2 - target.RectDst.Height / 2);
    }

    public Vector2 GetCursorePositionWorld()
    {
        Vector2 cursorePosition;
        MouseState _currentMouseState = Mouse.GetState();
        var mousePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
        cursorePosition = new Vector2(_currentMouseState.X - (int)Position.X, _currentMouseState.Y - (int)Position.Y);
        return cursorePosition;
    }

    public Vector2 GetCursorePositionUI()
    {
        Vector2 cursorePosition;
        MouseState _currentMouseState = Mouse.GetState();
        cursorePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
        return cursorePosition;
    }
}
