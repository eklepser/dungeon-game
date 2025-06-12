using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Venefica.Logic.Base;

internal class ControlManager
{
    private GameObjectCollidable target;
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;
    
    public ControlManager(GameObjectCollidable target)
    {
        this.target = target;
    }

    public void Update(Camera camera)
    {
        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();

        UpdateKeyboard(camera);
        UpdateMouse(camera);

        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;
    }

    private void UpdateKeyboard(Camera camera)
    {
        Vector2 inputVelocity = Vector2.Zero;
        if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentKeyboardState.IsKeyDown(Keys.W)) inputVelocity.Y -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentKeyboardState.IsKeyDown(Keys.S)) inputVelocity.Y += 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A)) inputVelocity.X -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D)) inputVelocity.X += 1;
        target.InputVelocity = inputVelocity;
    }

    private void UpdateMouse(Camera camera)
    {
        if (_currentMouseState.LeftButton == ButtonState.Pressed &&
        _previousMouseState.LeftButton == ButtonState.Released)
        {
            target.PositionPixels = camera.GetCursorePosition();
        }
    }
}
