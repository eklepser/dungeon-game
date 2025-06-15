using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Graphics;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.Base;

internal class Player : Entity
{
    public List<Item> Inventory = new();
    public Player(Sprite sprite, Vector2 position, int layer, EntityTemplate template) : base(sprite, position, layer, template) { }
}




