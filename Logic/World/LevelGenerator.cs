using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Venefica.Logic.World;

internal class LevelGenerator
{
    private Texture2D _tileMap;
    readonly private Vector2[] directionVectors = new Vector2[]
    {
        Vector2.Zero, // none
        new Vector2(0, -1), // up 
        new Vector2(1, 0),  // rigt
        new Vector2(0, 1),  // down
        new Vector2(-1, 0), // left
    };
    private int ReverseDirection(int direction) => (direction + 1) % 4 + 1;

    public LevelGenerator(string tileMapName, ContentManager content)
    {
        _tileMap = content.Load<Texture2D>(tileMapName);
    }

    public List<Room> Generate(int numMainRooms)
    {
        List<Room> rooms = new();
        Random random = new Random();
        bool noPlaceForRoom = false;

        // creating enter room
        Room enterRoom = new(Vector2.Zero, "room1", _tileMap);

        enterRoom.Load();

        System.Diagnostics.Debug.WriteLine($": enterRoom at {enterRoom.PositionWorld}");
        rooms.Add(enterRoom);

        // creating other rooms
        noPlaceForRoom = false;
        for (int depth = 0; depth < 4; depth++)
        {
            for (int i = depth; i < numMainRooms / Math.Pow(2, depth); i++)
            {
                int rndRoomIndex;
                Room currentRoom;
                if (noPlaceForRoom) rndRoomIndex = random.Next(i, rooms.Count);
                else rndRoomIndex = random.Next(depth * i + 1, depth * i + 1);
                if (i == 0) currentRoom = rooms[0];
                else currentRoom = rooms[rndRoomIndex];

                int rndPreset = random.Next(1, 4);
                Room newRoom = new(currentRoom.PositionWorld, "room" + rndPreset, _tileMap);

                int rndDirection = currentRoom.AvailableDirections[random.Next(currentRoom.AvailableDirections.Count)];
                newRoom.PositionWorld = GetRandomRoomPosition(currentRoom, newRoom, rndDirection);

                if (RoomIntersects(newRoom, rooms))
                {
                    noPlaceForRoom = true;
                    i--;
                    continue;
                }
                
                newRoom.Load();
                newRoom.RemoveWall(ReverseDirection(rndDirection));
                currentRoom.RemoveWall(rndDirection);

                System.Diagnostics.Debug.WriteLine($"{i}: new room at {newRoom.PositionWorld}");
                noPlaceForRoom = false;
                rooms.Add(newRoom);
            }
        }
        return rooms;
    }

    private bool RoomIntersects(Room targetRoom, List<Room> rooms)
    {
        foreach (Room room in rooms)
        {
            if (targetRoom.RectWorld.Intersects(room.RectWorld)) return true;
        }
        return false;
    }

    private Vector2 GetRandomRoomPosition(Room currentRoom, Room newRoom, int direction)
    {
        Vector2 newPosition = newRoom.PositionWorld;
        newPosition += new Vector2((int)(currentRoom.SizeWorld.X / 2), (int)(currentRoom.SizeWorld.Y / 2));
        newPosition -= new Vector2((int)(newRoom.SizeWorld.X / 2), (int)(newRoom.SizeWorld.Y / 2));

        
        newPosition += new Vector2(directionVectors[direction].X *
                                             (currentRoom.SizeWorld.X + newRoom.SizeWorld.X) / 2,
                                             directionVectors[direction].Y *
                                             (currentRoom.SizeWorld.Y + newRoom.SizeWorld.Y) / 2);
        return newPosition;
    }
}
