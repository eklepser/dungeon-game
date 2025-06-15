using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Entities;

internal class EntityTemplate
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int MaxHealth { get; set; }
    public float MoveSpeed { get; set; }
    public int DamageOnTouch { get; set; }
    public string WeaponName { get; set; }
    public int BaseAgressionLevel { get; set; }
    public int XpReward { get; set; }

    public string SpriteName { get; set; }
    public Sprite Sprite { get; set; }
    public Dictionary<string, Animation> AnimationSet { get; set; }
}
