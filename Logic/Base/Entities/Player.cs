using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RenderingLibrary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.InventoryLogic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal class Player : Entity
{
    //public List<Item> Inventory = new();
    public PlayerInventory Inventory;
    public Player(Sprite sprite, Vector2 position, int layer, EntityTemplate template) : base(sprite, position, layer, template) 
    {
        Inventory = new(3, 6);
    }

    public void Shoot(Vector2 position, ContentManager content, GameTime gameTime, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw)
    {
        for (int i = 0; i < Inventory.HandsAmount; i++)
        {
            if (Inventory.Hands[i] is Staff firstStaff) firstStaff.Shoot(position, content, gameTime, this, objectsForUpdate, objectsForDraw);
        }
    }
}




