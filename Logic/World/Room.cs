using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Venefica.Logic.Base;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.World;

internal class Room
{
    private string _preset;
    private string _root = "../../../World/Level1/";
    private Texture2D _tileMap;
    public List<int> AvailableDirections;
    public Vector2 PositionWorld { get; set; }
    public Vector2 SizeWorld { get; set; } = Vector2.Zero;
    public Vector2 CenterWorld
    {
        get => new((PositionWorld.X + SizeWorld.X) / 2, (PositionWorld.Y + SizeWorld.Y) / 2);
    }
    public Rectangle RectWorld
    {
        get => new((int)PositionWorld.X, (int)PositionWorld.Y, (int)SizeWorld.X, (int)SizeWorld.Y);
    }

    public Dictionary<Vector2, int> BackGround = new();
    public Dictionary<Vector2, int> MidGround = new();
    public Dictionary<Vector2, int> ForeGround = new();
    public Dictionary<Vector2, int> Collisions = new();

    public List<GameObject> RoomObjectsStatic = new();
    public List<GameObjectCollidable> RoomCollisions = new();

    public Room(Vector2 position, string preset, Texture2D tileMap)
    {
        PositionWorld = position;
        _preset = preset;
        _tileMap = tileMap;
        SizeWorld = CalculateSize(_root + _preset + "_bg.csv");
        AvailableDirections = new List<int> { 1, 2, 3 };
    } 

    public void Load()
    {
        if (File.Exists(_root + _preset + "_bg.csv"))
        {
            BackGround = LoadLayerMap(_root + _preset + "_bg.csv");
        }
        if (File.Exists(_root + _preset + "_mg.csv"))
        {
            MidGround = LoadLayerMap(_root + _preset + "_mg.csv");
        }
        if (File.Exists(_root + _preset + "_fg.csv"))
        {
            ForeGround = LoadLayerMap(_root + _preset + "_fg.csv");
        }
        if (File.Exists(_root + _preset + "_cl.csv"))
        {
            Collisions = LoadLayerMap(_root + _preset + "_cl.csv");
        }
    }

    public void Create()
    {
        CreateLayer(BackGround, -100);
      
        CreateLayer(MidGround, 10);
      
        CreateLayer(ForeGround, 100);
       
        CreateLayer(Collisions, 0);
    }

    public void LoadAndCreate()
    {
        Load();
        Create();
    }

    private void CreateLayer(Dictionary<Vector2, int> layerMap, int layer)
    {
        int numTilesPerRow = _tileMap.Height / Constants.TileSizeSrc;
        foreach (var item in layerMap)
        {
            int x = item.Value % numTilesPerRow;
            int y = item.Value / numTilesPerRow;

            Rectangle rectSrc = new(x, y, Constants.TileSizeSrc, Constants.TileSizeSrc);

            Rectangle rectDst = new(
                (int)(PositionWorld.X * Constants.TileSizeDst) + (int)item.Key.X * Constants.TileSizeDst,
                (int)(PositionWorld.Y * Constants.TileSizeDst) + (int)item.Key.Y * Constants.TileSizeDst,
                Constants.TileSizeDst,
                Constants.TileSizeDst);

            Sprite sprite = new(_tileMap, new Vector2(rectSrc.X, rectSrc.Y));
            GameObject gameObject;
            if (layer == 0)
            {
                gameObject = new Obstacle(sprite, new Vector2(rectDst.X, rectDst.Y), 0);
                RoomCollisions.Add((Obstacle)gameObject);
            }   
            else
            {
                gameObject = new GameObjectStatic(sprite, new Vector2(rectDst.X, rectDst.Y), layer);
                RoomObjectsStatic.Add(gameObject);
            }             
        }
    }

    private Dictionary<Vector2, int> LoadLayerMap(string filePath)
    {
        Dictionary<Vector2, int> result = new();
        StreamReader reader;

        try { reader = new(filePath); }
        catch { return result; }

        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++)
            {
                if (int.TryParse(items[x], out int value))
                {
                    if (value > -1)
                    {
                        result[new Vector2(x, y)] = value;
                    }
                }
            }
            y++;
        }
        return result;
    }

    private Vector2 CalculateSize(string filePath)
    {
        int lineCount = 0;
        int maxColumnCount = 0;

        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var columns = line.Split(',', StringSplitOptions.None);
                int columnCount = columns.Length;

                if (columnCount > maxColumnCount)
                    maxColumnCount = columnCount;

                lineCount++;
            }
        }
        return new Vector2(maxColumnCount, lineCount);
    }

    public void RemoveWall(int direction)
    {
        List<Vector2> tilePositions = new();

        switch (direction)
        {
            case 1:
                tilePositions.Add(new(SizeWorld.X / 2, 0));
                tilePositions.Add(new(SizeWorld.X / 2 - 1, 0));
                break;

            case 2:
                tilePositions.Add(new(SizeWorld.X - 1, SizeWorld.Y / 2));
                tilePositions.Add(new(SizeWorld.X - 1, SizeWorld.Y / 2 - 1));
                break;  

            case 3:
                tilePositions.Add(new(SizeWorld.X / 2, SizeWorld.Y - 1));
                tilePositions.Add(new(SizeWorld.X / 2 - 1, SizeWorld.Y - 1));
                break;

            case 4:
                tilePositions.Add(new(0, SizeWorld.Y / 2));
                tilePositions.Add(new(0, SizeWorld.Y / 2 - 1));
                break;

            default:
                break;
        }

        foreach (var tilePosition in tilePositions) RemoveTile(tilePosition);
    }

    private void RemoveTile(Vector2 tilePosition)
    {
        MidGround.Remove(tilePosition);
        ForeGround.Remove(tilePosition);
        Collisions.Remove(tilePosition);
    }

    private void ReplaceTile(Vector2 tilePosition)
    {
        MidGround.Remove(tilePosition);
        ForeGround.Remove(tilePosition);
        Collisions.Remove(tilePosition);
    }
}
