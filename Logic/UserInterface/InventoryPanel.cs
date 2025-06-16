using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using Venefica.Logic.Base;
using Venefica.Logic.Base.InventoryLogic;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal class InventoryPanel : UserInterfaceObject
{
    MouseState _currentMouseState;
    MouseState _previousMouseState;

    private Inventory _inventory;
    private float _firstHandLastAnimationTime;
    private float _secondHandLastAnimationTime;

    private Panel _inventoryPanel;
    private Cell _firstHand;
    private Cell _secondHand;

    public InventoryPanel(GumService gum, Inventory inventory)
    {
        IsVisible = true;
        _inventory = inventory;

        _inventoryPanel = new();
        _inventoryPanel.IsVisible = IsVisible;
        _inventoryPanel.Dock(Dock.Fill);
        _inventoryPanel.AddToRoot();

        _firstHand = new(0, 0, 60, 60, Color.DarkGray, 50);
        _firstHand.Anchor(Anchor.Bottom);
        _firstHand.X -= 100;     

        _secondHand = new(0, 0, 60, 60, Color.DarkGray, 50);
        _secondHand.Anchor(Anchor.Bottom);
        _secondHand.X += 100;
        
        _inventoryPanel.AddChild(_firstHand);
        _inventoryPanel.AddChild(_secondHand);
    }

    override public void Update(float deltaTime, Camera camera)
    {
        _firstHand.SetItem(_inventory.Hands[0]);
        _secondHand.SetItem(_inventory.Hands[1]);

        if (_inventory.Hands[0] is Staff staff)
        {
            // start animation
            if (_firstHandLastAnimationTime - staff.LastShootTime < 0)
            {
                _firstHand.Alpha = 200;
                _firstHandLastAnimationTime = staff.LastShootTime;
            }
            else if ((_firstHandLastAnimationTime == staff.LastShootTime) && (_firstHand.Alpha > 51)) _firstHand.Alpha -= (int)(staff.AttackSpeed * deltaTime * 200);
        }

        if (_inventory.Hands[1] is Staff staff1)
        {
            if (_secondHandLastAnimationTime - staff1.LastShootTime < 0)
            {
                _secondHand.Alpha = 200;
                _secondHandLastAnimationTime = staff1.LastShootTime;
            }
            else if ((_secondHandLastAnimationTime == staff1.LastShootTime) && (_secondHand.Alpha > 51)) _secondHand.Alpha -= (int)(staff1.AttackSpeed * deltaTime * 200);
        }

        Vector2 cursorPos = camera.GetCursorePositionUI();
        _currentMouseState = Mouse.GetState();
        if (_firstHand.IsPointInside(cursorPos.X, cursorPos.Y))
        {
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                if ((_firstHand.Item != null) && (_inventory.Dragged == null))
                {
                    _inventory.Dragged = _firstHand.Item;
                    _inventory.Hands[0] = null;
                }
                if ((_firstHand.Item == null) && (_inventory.Dragged != null))
                {
                    _inventory.Hands[0] = _inventory.Dragged;
                    _inventory.Dragged = null;
                }
            }
        }

        if (_secondHand.IsPointInside(cursorPos.X, cursorPos.Y))
        {
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                if ((_secondHand.Item != null) && (_inventory.Dragged == null))
                {
                    _inventory.Dragged = _secondHand.Item;
                    _inventory.Hands[1] = null;
                }
                if ((_secondHand.Item == null) && (_inventory.Dragged != null))
                {
                    _inventory.Hands[1] = _inventory.Dragged;
                    _inventory.Dragged = null;
                }
            }
        }

        _previousMouseState = _currentMouseState;
    }

    override public void ChangeVisibility()
    {
        IsVisible = !IsVisible;
        _inventoryPanel.IsVisible = IsVisible;
    }
}
