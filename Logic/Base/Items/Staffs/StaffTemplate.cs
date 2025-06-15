using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Venefica.Logic.Base.Weapons;

namespace Venefica.Logic.Base.Items.Staffs;

internal class StaffTemplate
{
    public string Name { get; set; }
    public string Type { get; set; }
    public float AttackSpeed { get; set; }
    public string SpriteName { get; set; }

    public Projectile Projectile { get; set; }
}
