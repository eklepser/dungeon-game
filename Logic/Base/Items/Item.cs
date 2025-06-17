using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Items;

internal abstract class Item
{
    public string Name { get; set; }
    public Sprite Sprite { get; set; }
    public string SpriteName { get; set; }
    virtual public string Description { get; set; }
}
