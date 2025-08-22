using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Items.Staffs;

internal class StaffTemplate
{
    public string Name { get; set; }
    public string Type { get; set; }
    public float AttackSpeed { get; set; }
    public string SpriteName { get; set; }
    public Sprite Sprite { get; set; }

    public Projectile Projectile { get; set; }
}
