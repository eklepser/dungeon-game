using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Venefica.Logic.Base;

namespace Venefica.Logic.Graphics;

internal class Sprite
{
    public Vector2 PosTileMap { get; set; }
    public readonly Texture2D Texture;
    public Rectangle RectSrc
    {
        get => new((int)PosTileMap.X * Constants.TileSizeSrc,
                   (int)PosTileMap.Y * Constants.TileSizeSrc,
                   Constants.TileSizeSrc,
                   Constants.TileSizeSrc);
    }

    // constructor for room's tilemaps
    public Sprite(Texture2D texture, Vector2 posTileMap)
    {
        Texture = texture;
        PosTileMap = posTileMap;
    }

    // constructor for animated tilemaps
    public Sprite(string textureName, ContentManager content)
    {
        Texture = content.Load<Texture2D>(textureName);
    }
}
