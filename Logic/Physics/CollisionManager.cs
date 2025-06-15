using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.Physics;

internal static class CollisionManager
{
    private readonly static int cellSize = 64; // grid cell size
    private static Dictionary<Point, List<GameObjectCollidable>> grid = new();

    public static void Update(float deltaTime, GameTime gameTime, List<GameObjectCollidable> allCollidables)
    {
        grid.Clear();
        
        foreach (var obj in allCollidables)
        {
            AddToGrid(obj);
        }

        foreach (var obj in allCollidables)
        {
            List<GameObjectCollidable> nearby = GetNearby(obj);

            // processing collisions
            foreach (var other in nearby)
            {
                if (obj.RectHitBoxBig.Intersects(other.RectHitBoxBig))
                {
                    HandleCollision(obj, other, gameTime);
                }
            }

            // calculating velocities
            if (obj is Player)
            {
                obj.Velocity = obj.InputVelocity;
                obj.PositionPixels += obj.Velocity * obj.BaseSpeed * deltaTime;
            }
            if (obj is Projectile) obj.PositionPixels += obj.Velocity * obj.BaseSpeed * deltaTime;
        }
    }
    
    private static void HandleCollision(GameObjectCollidable objA, GameObjectCollidable objB, GameTime gameTime)
    {
        if (objA == objB) return;

        // two entities processing
        if (objA is Entity entityA && objB is Entity entityB)
        {
            HandleEntitiesTouching(entityA, entityB, gameTime);
        }

        // projectile processing
        if (objA is Projectile projA)
        {
            HandleProjectileInteraction(projA, objB, gameTime);
        }

        if (objA is Projectile || objB is Projectile) return;

        // movement processing
        if (objA is Obstacle)
        {
            HandleCollisionWithObstacle(objB, (Obstacle)objA);
            return;
        }

        if (objB is Obstacle)
        {
            HandleCollisionWithObstacle(objA, (Obstacle)objB);
            return;
        }

        var rectA = objA.RectHitBoxSmall;
        var rectB = objB.RectHitBoxSmall;
        if (!rectA.Intersects(rectB)) return;

        HandleDynamicCollision(objA, objB);
    }

    private static void HandleEntitiesTouching(Entity entityA, Entity entityB, GameTime gameTime)
    {
        // exchange damage on touch
        if (entityA.CanAttackWithTouch(gameTime) && entityB is Player)
        {
            entityB.TakeDamage(entityA.DamageOnTouch, gameTime);
        }

        if (entityB.CanAttackWithTouch(gameTime) && entityA is Player)
        {
            entityA.TakeDamage(entityB.DamageOnTouch, gameTime);
        }
    }

    private static void HandleProjectileInteraction(Projectile projA, GameObjectCollidable objB, GameTime gameTime)
    {
        float now = (float)gameTime.TotalGameTime.TotalSeconds;

        if (objB is Obstacle && ((now - projA.lastBounceTime) > projA.bounceCooldown))
        {
            projA.lastBounceTime = now;
            projA.TakeDamage(1, gameTime);
            if (projA.CurrentHealth > 0)
            {
                Vector2 normal = CalculateWallNormal(projA, objB);
                projA.Velocity = Vector2.Normalize(Vector2.Reflect(projA.Velocity, normal)) * projA.MoveSpeed;
            }
        }
            
        else if (objB is Entity entityB && (objB != projA.Owner))
        {
            if (projA.CanAttackWithTouch(gameTime))
            {
                entityB.TakeDamage(projA.DamageOnTouch, gameTime);
            }
        }
    }

    private static void HandleCollisionWithObstacle(GameObjectCollidable dynamicObj, Obstacle obstacle)
    {
        Rectangle dynamicRect = dynamicObj.RectHitBoxBig;
        Rectangle obstacleRect = obstacle.RectHitBoxBig;

        Vector2 normal = new(obstacleRect.Center.X - dynamicRect.Center.X,
                             obstacleRect.Center.Y - dynamicRect.Center.Y);

        if (normal != Vector2.Zero)
            normal.Normalize();

        ResolvePenetration(dynamicObj, obstacle, normal, dynamicRect, obstacleRect);
    }

    private static void HandleDynamicCollision(GameObjectCollidable a, GameObjectCollidable b)
    {
        Vector2 centerA = new(a.RectHitBoxSmall.Center.X, a.RectHitBoxSmall.Center.Y);
        Vector2 centerB = new(b.RectHitBoxSmall.Center.X, b.RectHitBoxSmall.Center.Y);

        Vector2 normal = centerB - centerA;
        if (normal == Vector2.Zero) return;

        normal.Normalize();

        float v1n = Vector2.Dot(a.Velocity, normal);
        float v2n = Vector2.Dot(b.Velocity, normal);

        Vector2 tangent = new(-normal.Y, normal.X);

        float v1t = Vector2.Dot(a.Velocity, tangent);
        float v2t = Vector2.Dot(b.Velocity, tangent);

        Vector2[] newVelocities = ImpulseCalculation(
            new Vector2(v1n, 0),
            new Vector2(v2n, 0),
            a.Mass,
            b.Mass);

        a.Velocity = normal * newVelocities[0].X + tangent * v1t;
        b.Velocity = normal * newVelocities[1].X + tangent * v2t;

        ResolvePenetration(a, b, normal, a.RectHitBoxSmall, b.RectHitBoxSmall);
    }

    private static void ResolvePenetration(GameObjectCollidable dynamicObj, GameObjectCollidable obstacle, Vector2 normal, Rectangle dynamicRect, Rectangle obstacleRect)
    {
        int depthX = (dynamicRect.Width + obstacleRect.Width) / 2 - Math.Abs(dynamicRect.Center.X - obstacleRect.Center.X);
        int depthY = (dynamicRect.Height + obstacleRect.Height) / 2 - Math.Abs(dynamicRect.Center.Y - obstacleRect.Center.Y);

        if (depthX < depthY)
        {
            float penetration = depthX;
            if (normal.X > 0) dynamicObj.PositionPixels.X -= penetration;
            else dynamicObj.PositionPixels.X += penetration;
        }
        else
        {
            float penetration = depthY;
            if (normal.Y > 0) dynamicObj.PositionPixels.Y -= penetration;
            else dynamicObj.PositionPixels.Y += penetration;
        }
    }


    public static Vector2[] ImpulseCalculation(Vector2 v1, Vector2 v2, float m1, float m2)
    {
        Vector2[] vectors = new Vector2[2];
        vectors[0] = v1 * ((m1 - m2) / (m1 + m2)) + v2 * ((2 * m2) / (m1 + m2));
        vectors[1] = v1 * ((2 * m1) / (m1 + m2)) + v2 * ((m2 - m1) / (m1 + m2));
        return vectors;
    }

    private static void AddToGrid(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBoxBig;

        int startX = rect.X / cellSize;
        int endX = (rect.X + rect.Width) / cellSize;
        int startY = rect.Y / cellSize;
        int endY = (rect.Y + rect.Height) / cellSize;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Point cell = new(x, y);
                if (!grid.ContainsKey(cell))
                    grid[cell] = new List<GameObjectCollidable>();

                grid[cell].Add(obj);
            }
        }
    }

    private static List<GameObjectCollidable> GetNearby(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBoxBig;
        List<GameObjectCollidable> result = new List<GameObjectCollidable>();

        int startX = rect.X / cellSize;
        int endX = (rect.X + rect.Width) / cellSize;
        int startY = rect.Y / cellSize;
        int endY = (rect.Y + rect.Height) / cellSize;

        for (int x = startX - 1; x <= endX + 1; x++)
        {
            for (int y = startY - 1; y <= endY + 1; y++)
            {
                Point cell = new Point(x, y);
                if (grid.TryGetValue(cell, out var objects))
                {
                    result.AddRange(objects);
                }
            }
        }
        return result;
    }

    private static Vector2 CalculateWallNormal(GameObjectCollidable dynamicObj, GameObjectCollidable obstacle)
    {
        Rectangle rectA = dynamicObj.RectHitBoxBig;
        Rectangle rectB = obstacle.RectHitBoxBig;

        Vector2 centerA = new(rectA.Center.X, rectA.Center.Y);
        Vector2 centerB = new(rectB.Center.X, rectB.Center.Y);

        Vector2 normal = centerB - centerA;

        if (Math.Abs(normal.X) > Math.Abs(normal.Y))
        {
            if (normal.X > 0) return new Vector2(1, 0);
            else return new Vector2(-1, 0);
        }
        else
        {
            if (normal.Y > 0) return new Vector2(0, -1);
            else return new Vector2(0, 1);
        }
    }
}