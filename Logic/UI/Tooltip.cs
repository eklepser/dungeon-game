using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Collections.Generic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Venefica.Logic.UI;

internal class Tooltip
{
    private Item _item;
    public List<ColoredText> TextBox = new();

    public Tooltip(ContentManager content)
    {
        TextBox.Add(new ColoredText(Vector2.Zero, "aboba", Color.Red, content, "Arial14"));
        TextBox.Add(new ColoredText(Vector2.Zero, "QQQ", Color.Purple, content, "Arial14"));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < TextBox.Count; i++)
        {
            TextBox[i].Draw(spriteBatch, new Vector2(0, 100 + i * 20));
        }
    }
}
