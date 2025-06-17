using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Graphics;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UI;

internal class Slot
{
    private Sprite _sprite;
    public Vector2 Position { get; set; }
    public Item Item { get; set; }
    public float Alpha { get; set; }
    public bool IsEnabled { get; set; }
    public Rectangle RectDst
    {
        get => new((int)Position.X, (int)Position.Y, _sprite.Texture.Width * 2, _sprite.Texture.Height * 2);
    }

    public Slot(Vector2 position, Sprite sprite, Item item)
    {
        Position = position;
        _sprite = sprite;
        Item = item;
        Alpha = 0.8f;
        IsEnabled = true;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Alpha != 0) spriteBatch.Draw(_sprite.Texture, RectDst, _sprite.RectSrc, new Color(Color.White, Alpha));
        if (Item != null) spriteBatch.Draw(Item.Sprite.Texture, RectDst, _sprite.RectSrc, Color.White);
    }
}
