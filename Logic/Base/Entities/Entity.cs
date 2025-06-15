using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Base.Weapons;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Entities;

internal abstract class Entity : GameObjectCollidable
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

    public void Shoot(Vector2 direction, ContentManager content, GameObject owner, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw)
    {

        //if (Weapon is not null && Weapon is Staff staff) staff.Shoot();

        //Sprite projSprite = new("projectile1", content);
        //projSprite.TextureSize = 10;
        //Projectile proj = new Projectile(projSprite, owner.PositionPixels, 101, owner);

        //Vector2 directionNormalized = Vector2.Normalize(direction - owner.PositionPixels);
        //proj.Velocity = directionNormalized * proj.MoveSpeed;
        //proj.DamageOnTouch = 1;
        //proj.CurrentHealth = 2;
        //proj.DamageOnTouchCooldown = 0.2f;

        //objectsForUpdate.Add(proj);
        //objectsForDraw.Add(proj);
    }
}

