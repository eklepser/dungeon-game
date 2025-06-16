using FlatRedBall.Glue.StateInterpolation;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using MonoGameGum.Input;
using RenderingLibrary;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base;
using Venefica.Logic.Base.InventoryLogic;
using Venefica.Logic.Base.Items;


namespace Venefica.Logic.UserInterface;

internal class InventoryMenu : UserInterfaceObject
{
    MouseState _currentMouseState;
    MouseState _previousMouseState;

    private Inventory _inventory;
    private List<Cell> _backpackGrid = new();
    private Cell _draggedCell;

    private Panel _inventoryMenu;
    private ColoredRectangleRuntime _panelBackGround;

    public InventoryMenu(GumService gum, Inventory inventory)
    {
        IsVisible = false;
        _inventory = inventory;

        _inventoryMenu = new();
        _inventoryMenu.IsVisible = IsVisible;
        _inventoryMenu.Anchor(Anchor.Left);
        _inventoryMenu.AddToRoot();

        _draggedCell = new(0, 0, 60, 60, Color.DarkGray, alpha: 0);
        _draggedCell.Visible = IsVisible;

        _draggedCell.AddToRoot();

        for (int i = 0; i < 6; i++)
        {
            Cell cell = new(0, 62 * i, 60, 60, Color.DarkGray, alpha: 100);
            _inventoryMenu.AddChild(cell);
            _backpackGrid.Add(cell);
        }
    }

    override public void Update(float deltaTime, Base.Camera camera)
    {
        Vector2 cursorPos = camera.GetCursorePositionUI();
        _currentMouseState = Mouse.GetState();

        _draggedCell.SetItem(_inventory.Dragged);
        for (int i = 0; i < 6; i++)
        {
            Cell cell = _backpackGrid[i];
            cell.SetItem(_inventory.Backpack[i]);         
            if (cell.IsPointInside(cursorPos.X, cursorPos.Y))
            {
                cell.Alpha = 200;
                if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                {
                    // put item
                    if ((cell.Item is null) && (_draggedCell.Item is not null))
                    {
                        _inventory.Backpack[i] = _draggedCell.Item;
                        //_draggedCell.SetItem(null);
                        _inventory.Dragged = null;


                    }
                    // bring item
                    else if ((cell.Item is not null) && (_draggedCell.Item is null))
                    {
                        //_draggedCell.SetItem(cell.Item);
                        _inventory.Dragged = cell.Item;
                        _inventory.Backpack[i] = null;
                    }
                }

                //if ((currentMouseState.LeftButton == ButtonState.Pressed) && (_draggedCell.Item is not null))
                //{     
                //    cell.SetItem(_draggedCell.Item);
                //    _draggedCell.SetItem(null);
                //}
            }            
            else cell.Alpha = 100;       
        }

        if (_draggedCell != null)
        {
            _draggedCell.X = cursorPos.X;
            _draggedCell.Y = cursorPos.Y;
        }

        _previousMouseState = _currentMouseState;
    }

    override public void ChangeVisibility()
    {
        IsVisible = !IsVisible;
        _inventoryMenu.IsVisible = IsVisible;
        _draggedCell.Visible = IsVisible;
    }
}
