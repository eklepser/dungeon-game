using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items.Staffs;

namespace Venefica.Logic.Base.Items;

internal abstract class Staff : Weapon
{
    private Random _random = new();
    public string Type { get; set; }
    public float AttackSpeed { get; set; }
    public Projectile Projectile { get; set; }

    public float LastShootTime;
    public override string Description 
    { 
        get => $"{Name}\nDamage:{Projectile.DamageOnTouch}\nAttackSpeed: {AttackSpeed}\nProjectileSize: {Projectile.RectHitBoxBig.Width}"; 
    }

    public Staff(StaffTemplate template) 
    { 
        Name = template.Name;
        Type = template.Type;
        AttackSpeed = template.AttackSpeed;
        SpriteName = template.SpriteName;
        Projectile = template.Projectile;
    }

    virtual public void Shoot(Vector2 direction, ContentManager content, GameTime gameTime, Entity owner, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw) 
    {
        float now = (float)gameTime.TotalGameTime.TotalSeconds;
        if (now - LastShootTime >= (1 / AttackSpeed))
        {
            Projectile newProjectile = CreateProjectile(Projectile, owner);
            Vector2 directionNormalized = Vector2.Normalize(direction - owner.PositionPixelsCenter);
            Vector2 directionSpreaded = GetSpreadDirection(directionNormalized);
            newProjectile.Velocity = directionSpreaded * newProjectile.MoveSpeed;
            objectsForUpdate.Add(newProjectile);
            objectsForDraw.Add(newProjectile);
            LastShootTime = now;
        }
        else return;    
    }

    virtual protected Projectile CreateProjectile(Projectile projectileTemplate, Entity owner)
    {
        Projectile newProjectile = new Projectile(projectileTemplate.Sprite, owner.PositionPixelsCenter, 101, owner);
        newProjectile.MoveSpeed = projectileTemplate.MoveSpeed;
        newProjectile.CurrentHealth = projectileTemplate.MaxHealth;
        newProjectile.DamageOnTouch = projectileTemplate.DamageOnTouch;
        newProjectile.DamageOnTouchCooldown = 0.2f;
        return newProjectile;
    }

    private Vector2 GetSpreadDirection(Vector2 direction)
    {
        float spreadCoefX = 1 - (_random.Next(3, 20) / 100.0f);
        float spreadCoefY = 1 - (_random.Next(3, 20) / 100.0f);
        Vector2 spreadDirection = new(direction.X * spreadCoefX, direction.Y * spreadCoefY);
        return spreadDirection;
    }
}
