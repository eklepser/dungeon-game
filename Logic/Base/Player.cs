using Microsoft.Xna.Framework;
using Venefica.Logic.Graphics;
using System.Collections.Generic;

namespace Venefica.Logic.Base;

internal class Player : Entity
{
    public Player(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) 
    {
        AnimationManager = new(this);
    }  
}
