using Microsoft.Xna.Framework;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal class Entity : GameObjectCollidable
{
    public Entity(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) { }
}

