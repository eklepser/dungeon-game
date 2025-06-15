using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Venefica.Logic.Base.Weapons;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal abstract class GameObjectCollidable : GameObject
{
    // physics 
    public Vector2 Velocity {  get; set; }
    public Vector2 InputVelocity { get; set; }
    public float Mass { get; set; } = 1.0f;
    public float BaseSpeed { get; set; } = 200.0f;
    public Dictionary<string, Animation> AnimationSet { get; protected set; }
    public bool IsDead { get; set; } = false;

    // animations
    private Animation _currentAnimation;
    private float _timeElapsed;
    private int _currentFrameIndex;

    // game logic
    public int CurrentHealth { get; set; } = 1;
    public int DamageOnTouch { get; set; }
    public float DamageOnTouchCooldown { get; set; } = 0.5f;

    // time (pseudo-) constants
    protected float _lastAttackTime = 0f;
    protected float _lastTakenDamageTime = 0f;
    protected float _immunityTime = 0.03f;

    public Rectangle RectHitBoxBig
    {
        get => RectDst;
    }
    public Rectangle RectHitBoxSmall
    {
        get => new(RectDst.X + RectDst.X / 4, RectDst.Y + RectDst.Y / 4, RectDst.Width / 2, RectDst.Height / 2);
    }

    public GameObjectCollidable(Sprite sprite, Vector2 position, int layer) : base(sprite, position, layer) {}

    public void PlayAnimation(Animation animation)
    {
        if (_currentAnimation != animation)
        {
            _currentAnimation = animation;
            _currentFrameIndex = 0;
            _timeElapsed = 0f;
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (AnimationSet.ContainsKey(animationName))
        PlayAnimation(AnimationSet[animationName]);
    }

    protected void UpdateAnimation(GameTime gameTime)
    {
        if (_currentAnimation == null || _currentAnimation.Frames.Count == 0)
            return;

        _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeElapsed >= _currentAnimation.FrameDuration)
        {
            _currentFrameIndex++;
            _timeElapsed = 0f;

            if (_currentFrameIndex >= _currentAnimation.Frames.Count)
            {
                if (_currentAnimation.IsLooped)
                    _currentFrameIndex = 0;
                else
                    _currentFrameIndex = _currentAnimation.Frames.Count - 1;
            }
            Sprite.PosTileMap = _currentAnimation.Frames[_currentFrameIndex];
        }
    }

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
