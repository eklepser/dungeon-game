using System;
using System.Collections.Generic;
using Venefica.Logic.Graphics;
using Venefica.Logic.UI;

namespace Venefica.Logic.Base.Items;

internal abstract class Item
{
    public Tooltip Tooltip { get; set; }
    public string Name { get; set; }
    public Sprite Sprite { get; set; }
    public string SpriteName { get; set; }
    virtual public List<String> Description { get; set; } = new();
}
