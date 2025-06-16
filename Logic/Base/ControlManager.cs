using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderingLibrary;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items;
using Venefica.Logic.UserInterface;

namespace Venefica.Logic.Base;

internal static class ControlManager
{
    private static Player _target;
    private static KeyboardState _currentKeyboardState;
    private static KeyboardState _previousKeyboardState;
    private static MouseState _currentMouseState;
    private static MouseState _previousMouseState;
    private static List<GameObjectCollidable> _objectsForUpdate;
    private static List<GameObject> _objectsForDraw;
    private static ContentManager _content;

    public static void Initialize(Player target, ContentManager content, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw)
    {
        _target = target;
        _content = content;
        _objectsForUpdate = objectsForUpdate;
        _objectsForDraw = objectsForDraw;     
    }

    public static void Update(Camera camera, GameTime gameTime)
    {
        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();

        UpdateKeyboard(camera, gameTime);
        UpdateMouse(camera, gameTime);

        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;
    }

    private static void UpdateKeyboard(Camera camera, GameTime gameTime)
    {
        Vector2 inputVelocity = Vector2.Zero;
        if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentKeyboardState.IsKeyDown(Keys.W)) inputVelocity.Y -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentKeyboardState.IsKeyDown(Keys.S)) inputVelocity.Y += 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A)) inputVelocity.X -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D)) inputVelocity.X += 1;
        _target.InputVelocity = inputVelocity;

        if (_currentKeyboardState.IsKeyDown(Keys.F)  && _previousKeyboardState.IsKeyUp(Keys.F))
        {
            Entity entity = EntityManager.Create(camera.GetCursorePositionWorld(), "skeleton");
            _objectsForUpdate.Add(entity);
            _objectsForDraw.Add(entity);
        }

        if (_currentKeyboardState.IsKeyDown(Keys.Tab) && _previousKeyboardState.IsKeyUp(Keys.Tab))
        {
            if (UserInterfaceManager.UserInterfaceElements.ContainsKey("inventory_menu"))
            {
                UserInterfaceManager.UserInterfaceElements["inventory_menu"].ChangeVisibility();
            }
        }
    }

    private static void UpdateMouse(Camera camera, GameTime gameTime)
    {
        if (_currentMouseState.LeftButton == ButtonState.Pressed)
        {
            if (_target.Inventory.Hands[0] is Staff firstStaff) firstStaff.Shoot(camera.GetCursorePositionWorld(), _content, gameTime, _target, _objectsForUpdate, _objectsForDraw);
            if (_target.Inventory.Hands[1] is Staff secondStaff) secondStaff.Shoot(camera.GetCursorePositionWorld(), _content, gameTime, _target, _objectsForUpdate, _objectsForDraw);
        }

        if (MouseClicked(mouse => mouse.RightButton)) _target.PositionPixels = camera.GetCursorePositionWorld();
    }

    public static bool MouseClicked(Func<MouseState, ButtonState> getButtonState)
    {
        var current = getButtonState(_currentMouseState);
        var previous = getButtonState(_previousMouseState);
        _previousMouseState = _currentMouseState;
        return current == ButtonState.Pressed && previous == ButtonState.Released;
    }
}
