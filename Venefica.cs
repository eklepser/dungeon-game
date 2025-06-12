using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using Venefica.Logic.Base;
using Venefica.Logic.Graphics;
using Venefica.Logic.Physics;
using Venefica.Logic.World;
using Venefica.Logic.Base.Tools;

namespace Venefica;
    
public class Venefica : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private List<GameObject> _objectsForDraw = new();
    private List<GameObject> _fovObjectsForDraw = new();
    private List<GameObjectCollidable> _objectsForUpdate = new();

    private List<Room> _rooms = new();

    private Camera _camera;
    private ControlManager _controlManager;
    private CollisionManager _collisionManager;
    private LevelGenerator _levelGenerator;

    private FrameCounter _frameCounter = new FrameCounter();
    float FPS;
    float deltaTime;

    Player lulu;
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
        InitGraphics(new Vector2(Constants.ScreenWidth, Constants.ScreenHeight));

        Sprite luluSprite = new("player", Content);
        lulu = new Player(luluSprite, new Vector2(100, 100), 100);
        lulu.AnimationManager.LoadAllAnimations("player");

        arial14 = Content.Load<SpriteFont>("Arial14");
       

        _levelGenerator = new("tileset1", Content);
        _rooms = _levelGenerator.Generate(20);

        foreach (Room room in _rooms)
        {
            room.Create();
            _objectsForDraw.AddRange(room.RoomObjectsStatic);
            _objectsForUpdate.AddRange(room.RoomCollisions);
        }         

        _camera = new(lulu);
        _collisionManager = new();
        _controlManager = new(lulu);
        _objectsForDraw.Add(lulu);
        _objectsForUpdate.Add(lulu);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        _controlManager.Update(_camera);
        _camera.Update(new Vector2(Constants.ScreenWidth, Constants.ScreenHeight), lulu);

        debugText = $"FPS: {FPS}\nStaticObjects: {_objectsForDraw.Count()}\nCollisions: {_objectsForUpdate.Count()}"
            + $"\n\nPositionPixel: {lulu.PositionPixels}\nPositionWorld: {lulu.PositionWorld}"
            + $"\nCursorPosition: {_camera.GetCursorePosition()} \nFOV: {_camera.FOV}"
            + $"\n\nVelocity {new Vector2((float)Math.Round(lulu.Velocity.X, 3), (float)Math.Round(lulu.Velocity.Y, 3))}";

        _fovObjectsForDraw.Clear();
        foreach (var obj in _objectsForDraw)
        {
            if (obj.RectDst.Intersects(_camera.FOV)) _fovObjectsForDraw.Add(obj);
        }
        foreach (var obj in _objectsForUpdate)
        {
            if (obj.AnimationManager != null) obj.AnimationManager.Update(gameTime);
        }

        _collisionManager.Update(deltaTime, _objectsForUpdate);

        if (lulu.Velocity.Length() > 0) lulu.AnimationManager.Play(lulu.AnimationManager.Animations["Walk"]);
        else lulu.AnimationManager.Play(lulu.AnimationManager.Animations["Idle"]);

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
