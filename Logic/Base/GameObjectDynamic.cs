using Microsoft.Xna.Framework;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal class GameObjectDynamic : GameObjectCollidable
{
    // game logic
    public int CurrentHealth { get; set; } = 1;
    public int DamageOnTouch { get; set; }
    public float DamageOnTouchCooldown { get; set; } = 0.5f;

    // time (pseudo-) constants
    protected float _lastAttackTime = 0f;
    protected float _lastTakenDamageTime = 0f;
    protected float _immunityTime = 0.03f;

    public GameObjectDynamic(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) { }

    public bool CanAttackWithTouch(GameTime gameTime)
    {
        float now = (float)gameTime.TotalGameTime.TotalSeconds;

        if (now - _lastAttackTime >= DamageOnTouchCooldown)
        {
            _lastAttackTime = now;
            return true;
        }
        return false;
    }

    public void TakeDamage(int takenDamage, GameTime gameTime)
    {
        float now = (float)gameTime.TotalGameTime.TotalSeconds;

        if (now - _lastTakenDamageTime > _immunityTime)
        {
            _lastTakenDamageTime = now;
            CurrentHealth -= takenDamage;
            Color = Color.IndianRed;
        }
    }

    public void Update(GameTime gameTime)
    {
        UpdateAnimation(gameTime);
        if (CurrentHealth <= 0) IsDead = true;

        float now = (float)gameTime.TotalGameTime.TotalSeconds;
        if (now - _lastTakenDamageTime >= 0.1f)
        {
            Color = Color.White;
        }
    }
}
