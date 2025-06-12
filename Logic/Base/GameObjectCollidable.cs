using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal abstract class GameObjectCollidable : GameObject
{
    public Vector2 Velocity {  get; set; }
    public Vector2 InputVelocity { get; set; }
    public float Mass { get; set; } = 1.0f;
    public float BaseSpeed { get; set; } = 200.0f;
    public AnimationManager AnimationManager { get; protected set; }

    public Rectangle RectHitBox
    {
        get => RectDst;
    }
    public GameObjectCollidable(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) { }


    public virtual bool CheckCollision(GameObjectCollidable other)
    {
        return RectDst.Intersects(other.RectDst);
    }
}
