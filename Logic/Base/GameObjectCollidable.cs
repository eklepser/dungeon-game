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
}
