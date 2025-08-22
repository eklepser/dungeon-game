using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Entities;

internal abstract class Entity : GameObjectDynamic
{
    public string Name { get; protected set; }
    public string Type { get; set; }
    public int MaxHealth { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public int Damage { get; set; }
    public string WeaponName { get; set; }
    public Weapon Weapon { get; set; }
    public int BaseAgressionLevel { get; set; }
    public int XpReward { get; set; }

    public Entity(Sprite sprite, Vector2 position, int layer, EntityTemplate template) : base(sprite, position, layer) 
    {
        Name = template.Name;
        Type = template.Type;
        MaxHealth = template.MaxHealth;
        CurrentHealth = MaxHealth;
        MoveSpeed = template.MoveSpeed;
        DamageOnTouch = template.DamageOnTouch;
        WeaponName = template.WeaponName;
        BaseAgressionLevel = template.BaseAgressionLevel;
        XpReward = template.XpReward;
        AnimationSet = template.AnimationSet;
    }

    public void Shoot(Vector2 direction, ContentManager content, GameObject owner, List<Chest> objectsForUpdate, List<GameObject> objectsForDraw)
    {
        //if (Weapon is not null && Weapon is Staff staff) staff.Shoot(PositionPixelsCenter, content, this, objectsForUpdate, objectsForDraw);
    }
}

