using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items.Staffs;
using Venefica.Logic.Base.Weapons;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Items;

internal abstract class Staff : Weapon
{
    public string Type { get; set; }
    public float AttackSpeed { get; set; }
    public string SpriteName { get; set; }
    public Projectile Projectile { get; set; }
    public Staff(StaffTemplate template) 
    { 
        Name = template.Name;
        Type = template.Type;
        AttackSpeed = template.AttackSpeed;
        SpriteName = template.SpriteName;
        Projectile = template.Projectile;
    }
    virtual public void Shoot(Vector2 direction, ContentManager content, Entity owner, List<GameObjectCollidable> objectsForUpdate, List<GameObject> objectsForDraw) 
    {
        Projectile newProjectile = CreateProjectile(Projectile, owner);
        Vector2 directionNormalized = Vector2.Normalize(direction - owner.PositionPixelsCenter);
        newProjectile.Velocity = directionNormalized * newProjectile.MoveSpeed;
        objectsForUpdate.Add(newProjectile);
        objectsForDraw.Add(newProjectile);
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
}
