using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Venefica.Logic.Base;

namespace Venefica.Logic.Physics;

internal class Grid
{
    private int cellSize;
    private Dictionary<Point, List<Chest>> grid = new();

    public Grid(int cellSize)
    {
        this.cellSize = cellSize;
    }

    public void Clear()
    {
        grid.Clear();
    }

    public void Add(Chest obj)
    {
        Rectangle rect = obj.RectHitBoxBig;

        // Получаем координаты ячеек, которые занимает объект
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
                    grid[cell] = new List<Chest>();

                grid[cell].Add(obj);
            }
        }
    }

    public List<Chest> GetNearby(Chest obj)
    {
        Rectangle rect = obj.RectHitBoxBig;
        List<Chest> result = new();

        int startX = rect.X / cellSize;
        int endX = (rect.X + rect.Width) / cellSize;
        int startY = rect.Y / cellSize;
        int endY = (rect.Y + rect.Height) / cellSize;

        for (int x = startX - 1; x <= endX + 1; x++) // +1/-1 чтобы учесть соседние ячейки
        {
            for (int y = startY - 1; y <= endY + 1; y++)
            {
                Point cell = new(x, y);
                if (grid.TryGetValue(cell, out var objects))
                {
                    result.AddRange(objects.Where(o => o != obj)); // исключаем сам объект
                }
            }
        }

        return result;
    }
}
