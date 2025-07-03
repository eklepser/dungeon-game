using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Venefica.Logic.Graphics;
using Venefica.Logic.Base.Items;
using Microsoft.Xna.Framework.Content;

namespace Venefica.Logic.UI;

internal class Slot
{
    private Sprite _sprite;
    private Tooltip _tooltip;
    public Vector2 Position { get; set; }
    public Item Item { get; set; }
    public float Alpha { get; set; }
    public bool IsEnabled { get; set; }
    public Rectangle RectDst
    {
        get => new((int)Position.X, (int)Position.Y, _sprite.Texture.Width * 2, _sprite.Texture.Height * 2);
    }

    public Slot(ContentManager content, Vector2 position, Sprite sprite, Item item)
    {
        Position = position;
        _sprite = sprite;
        Item = item;
        _tooltip = new(content, this);
        Alpha = 0.8f;
        IsEnabled = true;
    }

    public void PlaceTooltip(Vector2 position)
    {
        _tooltip.Position = position;
    }

    public void Update()
    {
        if (Item != null) _tooltip.Update();
    }

    public void Update(Item item)
    {
        Item = item;
        if (item != null) _tooltip.Update(); 
    }

    public void ShowTooltip()
    {
        _tooltip.IsVisible = true;
    }

    public void HideTooltip()
    {
       _tooltip.IsVisible = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Alpha != 0) spriteBatch.Draw(_sprite.Texture, RectDst, _sprite.RectSrc, new Color(Color.White, Alpha));
        if (Item != null) spriteBatch.Draw(Item.Sprite.Texture, RectDst, _sprite.RectSrc, Color.White);
        if ((Item != null) && _tooltip.IsVisible) _tooltip.Draw(spriteBatch);
    }
}
