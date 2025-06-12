using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Venefica.Logic.Base;

namespace Venefica.Logic.Physics;

internal class CollisionManagerOld
{
    public void Update(List<GameObjectCollidable> allCollidables)
    {
        foreach (var targetCollidable in allCollidables)
        {
            GameObjectCollidable otherCollidable;
            if (targetCollidable is Entity)
            {
                otherCollidable = FindIntersects(targetCollidable, allCollidables);
                if (otherCollidable != null) ResolveCollision(targetCollidable, otherCollidable);
            }
        }
    }

    private GameObjectCollidable FindIntersects(GameObjectCollidable target, List<GameObjectCollidable> collidables)
    {
        foreach (var other in collidables)
        {
            if (other == target) continue;

            if (target.RectDst.Intersects(other.RectDst))
            {
                return other;
            }
        }
        return null;
    }

    private void ResolveCollision(GameObjectCollidable player, GameObjectCollidable obstacle)
    {
        // Вычисляем пересечение
        Rectangle intersection = Rectangle.Intersect(player.RectDst, obstacle.RectDst);

        if (intersection.IsEmpty)
            return;

        // Вычисляем направление проникновения
        int leftDist = obstacle.RectDst.Left - player.RectDst.Left;
        int rightDist = obstacle.RectDst.Right - player.RectDst.Right;
        int topDist = obstacle.RectDst.Top - player.RectDst.Top;
        int bottomDist = obstacle.RectDst.Bottom - player.RectDst.Bottom;

        // Находим минимальное расстояние для "выталкивания"
        int minDist = Math.Abs(leftDist);
        Vector2 correction = Vector2.Zero;

        // Сравниваем все 4 стороны
        if (Math.Abs(rightDist) < minDist)
        {
            correction = new Vector2(rightDist, 0);
            minDist = Math.Abs(rightDist);
        }
        if (Math.Abs(topDist) < minDist)
        {
            correction = new Vector2(0, topDist);
            minDist = Math.Abs(topDist);
        }
        if (Math.Abs(bottomDist) < minDist)
        {
            correction = new Vector2(0, bottomDist);
        }

        // Применяем коррекцию позиции
        player.PositionPixels += correction;
    }

}
