using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using RenderingLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using Venefica.Logic.Base;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal static class UserInterfaceManager
{
    public static Dictionary<string, UserInterfaceObject> UserInterfaceElements = new();

    private static MouseState _currentMouseState;
    private static MouseState _previousMouseState;

    public static void LoadAllUserInterfaceObjects(ContentManager content, GumService gum, Player player)
    {
        UserInterfaceObject inventoryMenu = new InventoryMenu(gum, player.Inventory);
        UserInterfaceElements.Add("inventory_menu", inventoryMenu);
        UserInterfaceObject chestMenu = new ChestMenu(gum, player.Inventory);
        UserInterfaceElements.Add("chest_menu", chestMenu);
    }
   

    public static void Update(float deltaTime, Base.Camera camera)
    {
        _currentMouseState = Mouse.GetState();
        Vector2 cursorPos = camera.GetCursorePositionUI();

        foreach (var item in UserInterfaceElements.Values)
        {
            item.Update(deltaTime, cursorPos);
        }

        _previousMouseState = _currentMouseState;
    }

    public static bool MouseClicked(Func<MouseState, ButtonState> getButtonState)
    {
        var current = getButtonState(_currentMouseState);
        var previous = getButtonState(_previousMouseState);
        _previousMouseState = _currentMouseState;
        return current == ButtonState.Pressed && previous == ButtonState.Released;
    }
}
