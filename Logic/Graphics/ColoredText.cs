using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venefica.Logic.Graphics;

internal class ColoredText
{
    public Vector2 Position { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; }
    public SpriteFont Font { get; set; }

    public ColoredText(Vector2 position, string text, Color color, SpriteFont spriteFont)
    {
        Position = position;
        Text = text;
        Color = color;
        Font = spriteFont;
    }

    public ColoredText(Vector2 position, string text, Color color, ContentManager content, string spriteFontName)
    {
        Position = position;
        Text = text;
        Color = color;
        Font = content.Load<SpriteFont>(spriteFontName);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.DrawString(Font, Text, position, Color);
    }
}
