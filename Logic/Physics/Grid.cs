using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Base;

namespace Venefica.Logic.Physics;

internal class Grid
{
    private int cellSize;
    private Dictionary<Point, List<GameObjectCollidable>> grid = new();

    public Grid(int cellSize)
    {
        this.cellSize = cellSize;
    }

    public void Clear()
    {
        grid.Clear();
    }

    public void Add(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBox;

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
                    grid[cell] = new List<GameObjectCollidable>();

                grid[cell].Add(obj);
            }
        }
    }

    public List<GameObjectCollidable> GetNearby(GameObjectCollidable obj)
    {
        Rectangle rect = obj.RectHitBox;
        List<GameObjectCollidable> result = new();

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
