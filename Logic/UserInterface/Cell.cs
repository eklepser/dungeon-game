using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.Forms;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.UserInterface;

internal class Cell : ColoredRectangleRuntime
{
    public Item Item { get; set; } = null;
    public TextBox TextBox { get; set; } = null;
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


        TextBox = new TextBox();
        TextBox.IsVisible = false;
        TextBox.IsReadOnly = true;
        TextBox.TextWrapping = TextWrapping.Wrap;

        TextBox.Width *= 2;
        TextBox.Height *= 5;

        this.AddChild(TextBox);
    }

    public void SetItem(Item item)
    {
        Item = item;
        if (item != null)
        {
            _currentItemImage.Source = item.SpriteName + ".png";
            TextBox.Text = item.Description;
        }
        else
        {
            _currentItemImage.Source = null;
            TextBox.IsVisible = false;
        }
           
    }

    public void ShowTooltip()
    {
        if (Item != null)
        {
            TextBox.IsVisible = true;
        }
    }

    public void HideTooltip()
    {
        TextBox.IsVisible = false;
    }
}
