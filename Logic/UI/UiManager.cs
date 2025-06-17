using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Venefica.Logic.Base;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UI;

internal static class UiManager
{
    private static MouseState _currentMouseState;
    private static MouseState _previousMouseState;

    public static Dictionary<string, UiObject> UiObjects = new();

    public static Slot DraggedSlot {  get; set; }

    public static Item[] SavedArray;
    public static int SavedArrayIndex;

    public static void Initialize(ContentManager content, Player player)
    {

        DraggedSlot = new(Vector2.Zero, new("slot", content, 32), null);
        DraggedSlot.Alpha = 0;

        BackpackMenu bpMenu = new(content, player);
        UiObjects.Add("backpack_menu", bpMenu);
        HandsMenu hMenu = new(content, player);
        UiObjects.Add("hands_menu", hMenu);
    }

    public static void Update(Camera camera, Player player, float deltaTime)
    {
        _currentMouseState = Mouse.GetState();
        Vector2 cursorPosition = camera.GetCursorePositionUI();

        DraggedSlotUpdate(cursorPosition);

        foreach (var uiObj in UiObjects.Values)
        {
            if (uiObj.IsEnabled) uiObj.Update(cursorPosition, player, deltaTime);
        }

        _previousMouseState = _currentMouseState;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var uiObj in UiObjects.Values)
        {
            if (uiObj.IsEnabled) uiObj.Draw(spriteBatch);
        }

        DraggedSlot.Draw(spriteBatch);
    }

    public static bool IsMouseClicked(Func<MouseState, ButtonState> getButtonState)
    {
        var current = getButtonState(_currentMouseState);
        var previous = getButtonState(_previousMouseState);
        return current == ButtonState.Pressed && previous == ButtonState.Released;
    }

    public static void SaveSlot(Item[] array, int index)
    {
        SavedArray = array;
        SavedArrayIndex = index;
    }

    private static void DraggedSlotUpdate(Vector2 cursorPosition)
    {
        DraggedSlot.Position = cursorPosition;
    }

    public static void SetVisibility(string uiElementName, bool isEnabled)
    {
        if (UiObjects.ContainsKey(uiElementName)) UiObjects[uiElementName].IsEnabled = isEnabled;
    }

    public static void SwitchVisibility(string uiElementName)
    {
        if (UiObjects.ContainsKey(uiElementName)) UiObjects[uiElementName].IsEnabled = !UiObjects[uiElementName].IsEnabled;
    }
}
