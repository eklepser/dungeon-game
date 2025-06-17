using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Base.Weapons;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal class Chest : GameObjectCollidable
{
    public Item[] Loot;
    new public Rectangle RectHitBoxBig
    {
        get => RectDst;
    }

    public Chest(Sprite sprite, Vector2 position, int layer, int inventorySize) : base(sprite, position, layer)
    {
        Loot = new Item[inventorySize];
        GenerateLoot();
    }

    public void GenerateLoot()
    {
        for (int i = 0; i < Loot.Length; i++)
        {
            Loot[i] = (Staff)ItemManager.Create("red_staff");
        }
    }
}



