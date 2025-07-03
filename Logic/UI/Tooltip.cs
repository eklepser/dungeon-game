using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using System.Collections.Generic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.UI;

internal class Tooltip
{
    private Slot _slot;
    private Item _item { get => _slot.Item; }
    private SpriteFont _font;
    public Vector2 Position { get; set; }
    public List<ColoredText> TextBox = new();
    public bool IsVisible = false;

    public Tooltip(ContentManager content, Slot slot)
    {
        _font = content.Load<SpriteFont>("Arial14");
        _slot = slot;
        Position = new Vector2(_slot.Position.X + _slot.RectDst.Width, _slot.Position.Y);
    }

    public void Update()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    {
        TextBox.Clear();
        TextBox.Add(new ColoredText(Position, _item.Description[0], Color.Red, _font));
        TextBox.Add(new ColoredText(Position, _item.Description[1], Color.Purple, _font));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < TextBox.Count; i++)
        {
            TextBox[i].Draw(spriteBatch, new Vector2(Position.X, Position.Y + i * 20));
        }
    }
}
