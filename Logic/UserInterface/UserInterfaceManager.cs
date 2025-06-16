using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using System.Collections.Generic;
using Venefica.Logic.Base;

namespace Venefica.Logic.UserInterface;

internal static class UserInterfaceManager
{
    public static Dictionary<string, UserInterfaceObject> UserInterfaceElements = new();
    public static void LoadAllUserInterfaceObjects(ContentManager content, GumService gum, Player player)
    {
        UserInterfaceObject inventoryPanel = new InventoryPanel(gum, player.Inventory);
        UserInterfaceElements.Add("inventory_panel", inventoryPanel);
        UserInterfaceObject inventoryMenu = new InventoryMenu(gum, player.Inventory);
        UserInterfaceElements.Add("inventory_menu", inventoryMenu);
    }
}
