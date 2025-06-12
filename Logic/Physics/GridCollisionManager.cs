using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Venefica.Logic.Base;

namespace Venefica.Logic.Physics;

internal class GridCollisionManager
{
    private readonly int cellSize;
    private readonly Dictionary<Point, List<GameObject>> grid = new();

    // Для ограничения области поиска
    private readonly Rectangle worldBounds;

    public GridCollisionManager(int cellSize, Rectangle worldBounds)
    {
        this.cellSize = cellSize;
        this.worldBounds = worldBounds;
    }

    public void Clear()
    {
        grid.Clear();
    }

    public void Insert(GameObjectCollidable obj)
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
                Point key = new(x, y);

                if (!grid.TryGetValue(key, out var list))
                {
                    list = new List<GameObject>();
                    grid[key] = list;
                }

                list.Add(obj);
            }
        }
    }

    public List<GameObject> GetPotentialColliders(GameObjectCollidable obj)
    {
        List<GameObject> result = new();

        Rectangle rect = obj.RectHitBox;

        int startX = rect.X / cellSize;
        int endX = (rect.X + rect.Width) / cellSize;

        int startY = rect.Y / cellSize;
        int endY = (rect.Y + rect.Height) / cellSize;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Point key = new(x, y);

                if (grid.TryGetValue(key, out var list))
                {
                    foreach (var other in list)
                    {
                        if (other != obj && !ReferenceEquals(other, obj))
                            result.Add(other);
                    }
                }
            }
        }

        return result;
    }
}