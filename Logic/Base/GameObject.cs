using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base;

internal abstract class GameObject
{
    public Sprite Sprite { get; set; }
    public Vector2 PositionPixels; // position in pixels
    public int Layer { get; set; } // defines the drawing queue place
    protected Color Color { get; set; } = Color.White;
    public Rectangle RectDst
    {
        get => new((int)PositionPixels.X,
                   (int)PositionPixels.Y, 
                   Constants.TileSizeDst * Sprite.TextureSize / Constants.TileSizeSrc, 
                   Constants.TileSizeDst * Sprite.TextureSize / Constants.TileSizeSrc);
    }
    public Vector2 PositionWorld
    {
        get => new Vector2(
        (float)Math.Round(PositionPixels.X / Constants.TileSizeDst, 3),
        (float)Math.Round(PositionPixels.Y / Constants.TileSizeDst, 3));
    }
    public Vector2 PositionPixelsCenter
    {
        get => new Vector2(
        PositionPixels.X + Sprite.TextureSize / 2,
        PositionPixels.Y + Sprite.TextureSize / 2);
    }

    public GameObject(Sprite sprite, Vector2 position, int layer)
    {
        Sprite = sprite;
        PositionPixels = position;
        Layer = layer;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle newRectDst = new Rectangle(
            (int)(RectDst.X + offset.X),
            (int)(RectDst.Y + offset.Y),
            RectDst.Size.X, RectDst.Size.Y);

        spriteBatch.Draw(Sprite.Texture, newRectDst, Sprite.RectSrc, Color);
    }
}
