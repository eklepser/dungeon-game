using Microsoft.Xna.Framework;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Graphics;
using Venefica.Logic.Base.InventoryLogic;

namespace Venefica.Logic.Base;

internal class Player : Entity
{
    //public List<Item> Inventory = new();
    public Inventory Inventory;
    public Player(Sprite sprite, Vector2 position, int layer, EntityTemplate template) : base(sprite, position, layer, template) 
    {
        Inventory = new();
    }
}




