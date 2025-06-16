using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal class Cell : ColoredRectangleRuntime
{
    public Item Item = null;
    private Image _currentItemImage;

    public Cell(int x, int y, int width, int height, Color color, int alpha = 100)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
        Alpha = alpha;

        _currentItemImage = new();
        _currentItemImage.Dock(Gum.Wireframe.Dock.Fill);
        this.AddChild(_currentItemImage);
    }

    public void SetItem(Item item)
    {
        Item = item;
        if (item != null)
        {
            _currentItemImage.Source = item.SpriteName + ".png";
            //this.AddChild(_currentItemImage);
        }
        else _currentItemImage.Source = null;
    }
}
