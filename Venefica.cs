using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using System.Collections.Generic;
using System.Linq;
using System;
using Venefica.Logic.Base;
using Venefica.Logic.Graphics;
using Venefica.Logic.Physics;
using Venefica.Logic.World;
using Venefica.Logic.Base.Tools;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Weapons;
using Venefica.Logic.Base.Items;
using Venefica.Logic.UserInterface;
using MonoGameGum.Forms;
using MonoGameGum.Forms.Controls.Primitives;
using MonoGameGum.Forms.DefaultVisuals;
using MonoGameGum.Forms.DefaultFromFileVisuals;
using MonoGameGum.Input;

namespace Venefica;
    
public class Venefica : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    GumService Gum => GumService.Default;

    private List<GameObject> _objectsForDraw = new();
    private List<GameObject> _fovObjectsForDraw = new();
    private List<GameObjectCollidable> _objectsForUpdate = new();

    private List<Room> _rooms = new();

    private Camera _camera;
    private LevelGenerator _levelGenerator;

    private FrameCounter _frameCounter = new FrameCounter();
    float FPS;
    float deltaTime;

    Entity kele;
    Player lulu;
    Projectile proj;
    SpriteFont arial14;
    string debugText;

    public Venefica()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
        _graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
    }

    protected override void Initialize()
    {
        Gum.Initialize(this);
        GumService.Default.CanvasWidth = 1280;
        GumService.Default.CanvasHeight = 720;
        //PauseMenuUI.CreatePauseMenu(Gum);

        AnimationManager.LoadAllAnimationSets();
        EntityManager.LoadAllTemplates(Content);
        ItemManager.LoadAllStaffTemplates(Content);

       
        Staff blueStaff = (Staff)ItemManager.Create("blue_staff");
        Staff redStaff1 = (Staff)ItemManager.Create("red_staff");
        Staff blueStaff1 = (Staff)ItemManager.Create("blue_staff");
        Staff blueStaff2 = (Staff)ItemManager.Create("blue_staff");

        InitGraphics(new Vector2(Constants.ScreenWidth, Constants.ScreenHeight));
        arial14 = Content.Load<SpriteFont>("Arial14");

        lulu = (Player)EntityManager.Create(new Vector2(100, 100), "player");

            

        //for (int i = 0; i < 15; i++)
        //{
        //    Staff redStaff = (Staff)ItemManager.Create("blue_staff");
        //    lulu.Inventory.Hands[i] = redStaff;
        //}

        Staff redStaff = (Staff)ItemManager.Create("red_staff");
        lulu.Inventory.Hands[0] = blueStaff;
        lulu.Inventory.Hands[1] = blueStaff1;
        lulu.Inventory.Backpack[1] = blueStaff2;
        lulu.Inventory.Backpack[2] = redStaff1;

        _objectsForDraw.Add(lulu);
        _objectsForUpdate.Add(lulu);

        ControlManager.Initialize(lulu, Content, _objectsForUpdate, _objectsForDraw);
        UserInterfaceManager.LoadAllUserInterfaceObjects(Content, Gum, lulu);


        Chest chest = new(null, Vector2.Zero, 100, 3);
        chest.GenerateLoot();

        if (UserInterfaceManager.UserInterfaceElements.ContainsKey("chest_menu"))
        {
            ChestMenu cm = (ChestMenu)UserInterfaceManager.UserInterfaceElements["chest_menu"];

            cm.UpdateContent(chest.Loot);
            foreach (var item in cm._inventory) System.Diagnostics.Debug.WriteLine(item);
        }

        kele = EntityManager.Create(new Vector2(100, 100), "skeleton");
        _objectsForDraw.Add(kele);
        _objectsForUpdate.Add(kele);

        Sprite projSprite = new("projectile1", Content);
        projSprite.TextureSize = 10;
        proj = new Projectile(projSprite, new Vector2(100, 200), 101, lulu);
        _objectsForDraw.Add(proj);
        _objectsForUpdate.Add(proj);

        _levelGenerator = new("tileset1", Content);
        _rooms = _levelGenerator.Generate(20);
        foreach (Room room in _rooms)
        {
            room.Create();
            _objectsForDraw.AddRange(room.RoomObjectsStatic);
            _objectsForUpdate.AddRange(room.RoomCollisions);
        }         

        _camera = new(lulu);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        ControlManager.Update(_camera, gameTime);

        //string inv = "\n";
        //foreach (var item in lulu.Inventory.Hands)
        //{
        //    if (item is Staff staff)
        //    inv += staff.Name + "   " + staff.Projectile.SpriteName + "\n";
        //}
        //debugText = $"FPS: {FPS}\nStaticObjects: {_objectsForDraw.Count()}\nCollisions: {_objectsForUpdate.Count()}"
        //    + $"\n\nPositionPixel: {lulu.PositionPixels}\nPositionWorld: {lulu.PositionWorld}"
        //    + $"\nCursorPosition: {_camera.GetCursorePositionWorld()} \nFOV: {_camera.FOV}"
        //    + $"\n\nVelocity {new Vector2((float)Math.Round(lulu.Velocity.X, 3), (float)Math.Round(lulu.Velocity.Y, 3))}"
        //    + $"\nInventory: " + inv;

        debugText = $"FPS: {FPS}";
        for (int i = 0; i < _objectsForUpdate.Count; i++)
        {
            var obj = _objectsForUpdate[i];
            if (obj is GameObjectDynamic dynamicObj)
            {
                dynamicObj.Update(gameTime);
                if (dynamicObj.IsDead)
                {
                    _objectsForUpdate.RemoveAt(i);
                    _objectsForDraw.Remove(dynamicObj);
                    i--;
                }
            }
        }

        _fovObjectsForDraw.Clear();
        foreach (var obj in _objectsForDraw)
        {
            if (obj.RectDst.Intersects(_camera.FOV)) _fovObjectsForDraw.Add(obj);
        }

        CollisionManager.Update(deltaTime, gameTime, _objectsForUpdate);
        UserInterfaceManager.Update(deltaTime, _camera);
        _camera.Update(new Vector2(Constants.ScreenWidth, Constants.ScreenHeight), lulu);

        Gum.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCounter.Update(deltaTime);
        FPS = _frameCounter.AverageFramesPerSecond;

        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        foreach (var gameObject in _fovObjectsForDraw.OrderBy(o => o.Layer).ThenBy(o => o.RectDst.Bottom))
        {
            gameObject.Draw(_spriteBatch, _camera.Position);
        }

        _spriteBatch.DrawString(arial14, debugText, Vector2.Zero, Color.YellowGreen);

        _spriteBatch.End();
        Gum.Draw();
        base.Draw(gameTime);
    }

    private void InitGraphics(Vector2 windowSize)
    {     
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = (int)windowSize.X;
        _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
        _graphics.ApplyChanges();
    }
}
