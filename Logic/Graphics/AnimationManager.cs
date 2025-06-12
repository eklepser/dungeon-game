using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Venefica.Logic.Base;

namespace Venefica.Logic.Graphics;

internal class AnimationManager
{
    private static string _folderPath = "../../../Animations/";
    private Animation _currentAnimation;
    private float _timeElapsed;
    private int _currentFrameIndex;
    private GameObjectCollidable _target;
    public Dictionary<string, Animation> Animations { get; } = new();

    public AnimationManager(GameObjectCollidable target)
    {
        _target = target;
    }

    public Vector2 CurrentFrame => _currentAnimation?.Frames[_currentFrameIndex] ?? Vector2.Zero;

    public void Play(Animation animation)
    {
        if (_currentAnimation != animation)
        {
            _currentAnimation = animation;
            _currentFrameIndex = 0;
            _timeElapsed = 0f;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (_currentAnimation == null || _currentAnimation.Frames.Count == 0)
            return;

        _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeElapsed >= _currentAnimation.FrameDuration)
        {
            _currentFrameIndex++;
            _timeElapsed = 0f;

            if (_currentFrameIndex >= _currentAnimation.Frames.Count)
            {
                if (_currentAnimation.IsLooped)
                    _currentFrameIndex = 0;
                else
                    _currentFrameIndex = _currentAnimation.Frames.Count - 1;
            }
            _target.Sprite.PosTileMap = _currentAnimation.Frames[_currentFrameIndex];
        }
    }

    public void LoadAllAnimations(string prefix)
    {
        foreach (var filePath in Directory.GetFiles(_folderPath, $"{prefix}*.json"))
        {
            var animation = LoadAnimation(filePath);
            Animations.Add(animation.Name, animation);
        }
    }

    private Animation LoadAnimation(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Animation file not found: {fileName}");

        string jsonText = File.ReadAllText(fileName);
        JObject jsonData = JObject.Parse(jsonText);

        Animation animation = new();

        // Читаем параметры
        animation.Name = (string)jsonData["name"];
        animation.FrameDuration = (float)jsonData["frameDuration"]!.ToObject<double>();
        animation.IsLooped = (bool)jsonData["isLooped"];     

        // Читаем кадры
        foreach (var frameToken in jsonData["frames"]!)
        {
            int x = (int)frameToken["x"]!;
            int y = (int)frameToken["y"]!;

            animation.Frames.Add(new Vector2(x, y));
        }
        return animation;
    }
}