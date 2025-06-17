using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venefica.Logic.Base.Items;

internal abstract class Item
{
    public string Name { get; set; }
    public string SpriteName { get; set; }
    virtual public string Description { get; set; }
}
