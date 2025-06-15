using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.Base;

internal class ControlManager
{
    private Player target;
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;
    private List<GameObjectCollidable> _objectsForUpdate;
    private List<GameObject> _objectsForDraw;
    private ContentManager _content;

    public ControlManager(Player target, ContentManager content, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw)
    {
        this.target = target;
        _objectsForUpdate = objectsForUpdate;
        _objectsForDraw = objectsForDraw;     
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

        if (_currentKeyboardState.IsKeyDown(Keys.F)  && _previousKeyboardState.IsKeyUp(Keys.F))
        {
            Entity entity = EntityManager.Create(camera.GetCursorePosition(), "skeleton");
            _objectsForUpdate.Add(entity);
            _objectsForDraw.Add(entity);
        }
    }

    private void UpdateMouse(Camera camera)
    {
        if (_currentMouseState.LeftButton == ButtonState.Pressed &&
        _previousMouseState.LeftButton == ButtonState.Released)
        {
            if (target.Inventory[0] is Staff staff) staff.Shoot(camera.GetCursorePosition(), _content, target, _objectsForUpdate, _objectsForDraw);
        }

        if (_currentMouseState.RightButton == ButtonState.Pressed &&
        _previousMouseState.RightButton == ButtonState.Released)
        {
            target.PositionPixels = camera.GetCursorePosition();
        }
    }
}
