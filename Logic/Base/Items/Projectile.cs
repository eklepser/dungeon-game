using Microsoft.Xna.Framework;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Items;

internal class Projectile : GameObjectDynamic
{
    public GameObject Owner { get; set;  }
    public float MoveSpeed { get; set; } = 4;
    public int MaxHealth { get; set; }
    public float bounceCooldown { get; set; } = 0.01f;
    public float lastBounceTime { get; set; } = 0;
    public string SpriteName;
    public Projectile(Sprite sprite, Vector2 position, int layer, GameObject owner) : base(sprite, position, layer) 
    { 
        Owner = owner;
    }
}
