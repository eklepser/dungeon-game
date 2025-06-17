using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Base;
using Venefica.Logic.Base.InventoryLogic;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal class ChestMenu : UserInterfaceObject
{
    private List<Cell> _allCells = new();
    private Panel _chestPanel;
    private PlayerInventory _playerInventory;
    public Item[] _inventory;

    public ChestMenu(GumService gum, PlayerInventory playerInventory)
    {        
        _playerInventory = playerInventory;

        IsVisible = true;

        _chestPanel = new();
        _chestPanel.IsVisible = true;
        _chestPanel.Anchor(Anchor.Center);
        _chestPanel.AddToRoot();

        for (int i = 0; i < 3; i++)
        {
            Cell cell = new(240 * i, -240, 120, 120, Color.DarkGray, alpha: 100);
            cell.Visible = true;
            cell.TextBox.Y += cell.Height;
            _chestPanel.AddChild(cell);
            _allCells.Add(cell);
        }
    }

    public override void Update(float deltaTime, Vector2 cursorPos)
    {
        for (int i = 0; i < 3; i++)
        {
            Cell cell = _allCells[i];
            cell.SetItem(_inventory[i]);
            if (cell.IsPointInside(cursorPos.X, cursorPos.Y))
            {
                cell.ShowTooltip();
                if (UserInterfaceManager.MouseClicked(mouse => mouse.LeftButton))
                {
                    _playerInventory.Backpack[0] = cell.Item;
                    _inventory = new Item[3];
                    IsVisible = false;
                }
                //ActionOnFocus(cell, i);
            }
            else cell.HideTooltip();
        }
    }

    public void UpdateContent(Item[] inventory)
    {
        _inventory = inventory;
    }

}
