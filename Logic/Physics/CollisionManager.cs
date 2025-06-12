using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Venefica.Logic.Base;

namespace Venefica.Logic.Physics;

internal class CollisionManager
{
    private readonly int cellSize = 64; // размер ячейки сетки
    private Dictionary<Point, List<GameObjectCollidable>> grid = new();
    List<GameObjectCollidable> collidables;

    public void Update(float deltaTime, List<GameObjectCollidable> allCollidables)
    {
        grid.Clear();

        float damping = 0.95f;
        
        foreach (var obj in allCollidables)
        {
            AddToGrid(obj);
        }

        foreach (var obj in allCollidables)
        {
            List<GameObjectCollidable> nearby = GetNearby(obj);

            foreach (var other in nearby)
            {
                if (obj.RectHitBox.Intersects(other.RectHitBox))
                {
                    HandleCollision(obj, other);
                }
            }

            obj.Velocity = obj.InputVelocity;
            obj.PositionPixels += obj.Velocity * obj.BaseSpeed * deltaTime;
        }
    }

    private void AddToGrid(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBox;

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

    private List<GameObjectCollidable> GetNearby(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBox;
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

    private void HandleCollision(GameObjectCollidable a, GameObjectCollidable b)
    {
        if (a == b) return;

        // Проверяем, является ли один из объектов препятствием
        if (a is Obstacle || a.GetType().IsSubclassOf(typeof(Obstacle)))
        {
            HandleCollisionWithObstacle(b, (Obstacle)a);
            return;
        }

        if (b is Obstacle || b.GetType().IsSubclassOf(typeof(Obstacle)))
        {
            HandleCollisionWithObstacle(a, (Obstacle)b);
            return;
        }

        // Ниже — стандартная обработка столкновений между двумя динамическими объектами

        Vector2 centerA = new(a.RectHitBox.Center.X, a.RectHitBox.Center.Y);
        Vector2 centerB = new(b.RectHitBox.Center.X, b.RectHitBox.Center.Y);

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
            b.Mass
        );

        a.Velocity = normal * newVelocities[0].X + tangent * v1t;
        b.Velocity = normal * newVelocities[1].X + tangent * v2t;

        ResolvePenetration(a, b, normal);
    }

    private void HandleCollisionWithObstacle(GameObjectCollidable dynamicObj, Obstacle obstacle)
    {
        Rectangle dynamicRect = dynamicObj.RectHitBox;
        Rectangle obstacleRect = obstacle.RectHitBox;

        Vector2 normal = new(obstacleRect.Center.X - dynamicRect.Center.X,
                             obstacleRect.Center.Y - dynamicRect.Center.Y);

        if (normal != Vector2.Zero)
            normal.Normalize();

        ResolvePenetration(dynamicObj, obstacle, normal);
    }

    private void ResolvePenetration(GameObjectCollidable dynamicObj, GameObjectCollidable obstacle, Vector2 normal)
    {
        Rectangle dynamicRect = dynamicObj.RectHitBox;
        Rectangle obstacleRect = obstacle.RectHitBox;

        int depthX = (dynamicRect.Width + obstacleRect.Width) / 2 - Math.Abs(dynamicRect.Center.X - obstacleRect.Center.X);
        int depthY = (dynamicRect.Height + obstacleRect.Height) / 2 - Math.Abs(dynamicRect.Center.Y - obstacleRect.Center.Y);

        if (depthX < depthY)
        {
            float penetration = depthX;
            if (normal.X > 0)
            {
                dynamicObj.PositionPixels.X -= penetration;
            }
            else
            {
                dynamicObj.PositionPixels.X += penetration;
            }
        }
        else
        {
            float penetration = depthY;
            if (normal.Y > 0)
            {
                dynamicObj.PositionPixels.Y -= penetration;
            }
            else
            {
                dynamicObj.PositionPixels.Y += penetration;
            }
        }

        // Опционально: обнуляем скорость по направлению нормали
        // dynamicObj.velocity = Vector2.Reflect(dynamicObj.velocity, normal) * 0.5f; // эффект отскока
    }

    public Vector2[] ImpulseCalculation(Vector2 v1, Vector2 v2, float m1, float m2)
    {
        Vector2[] vectors = new Vector2[2];
        vectors[0] = v1 * ((m1 - m2) / (m1 + m2)) + v2 * ((2 * m2) / (m1 + m2));
        vectors[1] = v1 * ((2 * m1) / (m1 + m2)) + v2 * ((m2 - m1) / (m1 + m2));
        return vectors;
    }
}