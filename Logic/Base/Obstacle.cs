using Microsoft.Xna.Framework;
using Venefica.Logic.Graphics;


namespace Venefica.Logic.Base;

internal class Obstacle : GameObjectCollidable
{
    public Obstacle(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) { }
}
