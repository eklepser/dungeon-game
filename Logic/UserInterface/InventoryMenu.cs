using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using RenderingLibrary.Graphics;
using System.Collections.Generic;
using System.Net;
using Venefica.Logic.Base.InventoryLogic;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal class InventoryMenu : UserInterfaceObject
{
    private int _backpackSize;
    private int _handsAmount;

    private PlayerInventory _inventory;
    private Cell _draggedCell;
    private int _savedIndex;

    private Panel _backpackPanel;
    private Panel _handsPanel;
    private List<Cell> _allCells = new();
    private List<float> _lastAnimTimeList = new();

    public InventoryMenu(GumService gum, PlayerInventory inventory)
    {
        _backpackSize = inventory.BackpackSize;
        _handsAmount = inventory.HandsAmount;

        IsVisible = false;
        _inventory = inventory;

        _draggedCell = new(0, 0, 60, 60, Color.DarkGray, alpha: 0);
        _draggedCell.Visible = true;
        _draggedCell.TextBox.Y -= (_draggedCell.Height + _draggedCell.TextBox.Height / 2);
        _draggedCell.AddToRoot();

        _backpackPanel = new();
        _backpackPanel.IsVisible = false;
        _backpackPanel.Anchor(Anchor.Left);
        _backpackPanel.AddToRoot();

        for (int i = 0; i < _backpackSize; i++)
        {
            Cell cell = new(0, 62 * i, 60, 60, Color.DarkGray, alpha: 100);
            cell.Visible = true;
            cell.TextBox.X += cell.Width;
            _backpackPanel.AddChild(cell);
            _allCells.Add(cell);
        }

        _handsPanel = new();
        _handsPanel.IsVisible = true;
        _handsPanel.Anchor(Anchor.Bottom);
        _handsPanel.AddToRoot();

        for (int i = 0; i < _handsAmount; i++)
        {
            Cell hand = new(0, 0, 60, 60, Color.DarkGray, 50);
            hand.X = 100 * i;
            hand.Visible = true;
            hand.TextBox.Y -= (hand.Height + hand.TextBox.Height / 2);
            _handsPanel.AddChild(hand);
            _allCells.Add(hand);
            _lastAnimTimeList.Add(0);
        }
    }

    override public void Update(float deltaTime, Vector2 cursorPos)
    {
        _draggedCell.SetItem(_inventory.Dragged);
        if (_draggedCell.Item != null) _draggedCell.ShowTooltip();          

        for (int i = 0; i < _allCells.Count; i++)
        {
            Cell cell = _allCells[i];

            if (i < _backpackSize) cell.SetItem(_inventory.Backpack[i]);   
            else cell.SetItem(_inventory.Hands[i - _backpackSize]);

            if (cell.IsPointInside(cursorPos.X, cursorPos.Y))  
            {
                ActionOnFocus(cell, i);
            }
            else
            {
                cell.HideTooltip();
                if (i < _backpackSize) cell.Alpha = 100;
            }
        }

        UpdateDraggedCell(cursorPos);
        UpdateHandsAnimations(deltaTime);
    }

    private void ActionOnFocus(Cell cell, int i)
    {
        cell.ShowTooltip();

        if (i < _backpackSize) cell.Alpha = 200;
        else if (cell.Item != null) _draggedCell.HideTooltip();

        if (UserInterfaceManager.MouseClicked(mouse => mouse.LeftButton))
        {

            // put item
            if ((cell.Item == null) && (_draggedCell.Item != null))
            {
                if ((i < _backpackSize) && IsVisible)
                {
                    _inventory.Backpack[i] = _draggedCell.Item;
                    _inventory.Dragged = null;
                }
                else if (i >= _backpackSize)
                {
                    _inventory.Hands[i - _backpackSize] = _draggedCell.Item;
                    _inventory.Dragged = null;
                }

            }

            // bring item
            else if ((cell.Item != null) && (_draggedCell.Item == null))
            {
                if ((i < _backpackSize) && IsVisible)
                {
                    _inventory.Dragged = cell.Item;
                    _inventory.Backpack[i] = null;
                    _savedIndex = i;
                }
                else if (i >= _backpackSize)
                {
                    _inventory.Dragged = cell.Item;
                    _inventory.Hands[i - _backpackSize] = null;
                    _savedIndex = i;
                }
            }

            // swap items
            else if ((cell.Item != null) && (_draggedCell.Item != null))
            {
                if (_savedIndex < _backpackSize)
                {
                    if ((i < _backpackSize) && IsVisible)
                    {
                        _inventory.Backpack[_savedIndex] = _inventory.Backpack[i];
                        _inventory.Backpack[i] = _inventory.Dragged;
                        _inventory.Dragged = null;
                    }
                    else if (i >= _backpackSize)
                    {
                        _inventory.Backpack[_savedIndex] = _inventory.Hands[i - _backpackSize];
                        _inventory.Hands[i - _backpackSize] = _inventory.Dragged;
                        _inventory.Dragged = null;
                    }
                }
                else
                {
                    _savedIndex -= _backpackSize;
                    if ((i < _backpackSize) && IsVisible)
                    {
                        _inventory.Hands[_savedIndex] = _inventory.Backpack[i];
                        _inventory.Backpack[i] = _inventory.Dragged;
                        _inventory.Dragged = null;
                    }
                    else if (i >= _backpackSize)
                    {
                        _inventory.Hands[_savedIndex] = _inventory.Hands[i - _backpackSize];
                        _inventory.Hands[i - _backpackSize] = _inventory.Dragged;
                        _inventory.Dragged = null;
                    }
                }
            }
        }
    }

    private void UpdateHandsAnimations(float deltaTime)
    {
        for (int i = 0; i < _handsAmount; i++)
        {
            if (_inventory.Hands[i] is Staff staff)
            {
                // start animation
                if (_lastAnimTimeList[i] - staff.LastShootTime < 0)
                {
                    _allCells[_backpackSize + i].Alpha = 200;
                    _lastAnimTimeList[i] = staff.LastShootTime;
                }
                else if ((_lastAnimTimeList[i] == staff.LastShootTime) && (_allCells[_backpackSize + i].Alpha > 51))
                {
                    _allCells[_backpackSize + i].Alpha -= (int)(staff.AttackSpeed * deltaTime * 200);
                }
            }
        }
    }

    private void UpdateDraggedCell(Vector2 cursorPos)
    {
        if (_draggedCell.Item != null)
        {
            _draggedCell.X = cursorPos.X;
            _draggedCell.Y = cursorPos.Y;
        }

        else _draggedCell.HideTooltip();
    }

    public void ChangeMenuVisibility()
    {
        IsVisible = !IsVisible;
        _backpackPanel.IsVisible = IsVisible;
        //if (IsVisible == true) _handsPanel.IsVisible = IsVisible;
    }

    public void ChangePanelVisibility()
    {
        _handsPanel.IsVisible = true;
    }
}
