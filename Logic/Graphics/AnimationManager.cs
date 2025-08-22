using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Venefica.Logic.Graphics;

internal static class AnimationManager
{
    private static readonly string _folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Animations");
    private static Dictionary<string, Dictionary<string, Animation>> _allAnimationSets = new();

    public static Dictionary<string, Animation> GetAnimationSet(string animationSetName)
    {
        return _allAnimationSets[animationSetName];
    }

    public static void LoadAllAnimationSets()
    {
        foreach (var filePath in Directory.GetFiles(_folderPath, $"*.json"))
        {
            var animationSet = LoadAnimationSet(filePath);
            var animationSetName = Path.GetFileNameWithoutExtension(filePath);
            if (!_allAnimationSets.ContainsKey(animationSetName))
                _allAnimationSets.Add(animationSetName, animationSet);
        }
    }

    private static Dictionary<string, Animation> LoadAnimationSet(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Entity template file not found: {fileName}");

        string jsonText = File.ReadAllText(fileName);
        var jsonData = JObject.Parse(jsonText);

        var animationSet = new Dictionary<string, Animation>();

        // Получаем секцию "animations"
        var animationsToken = jsonData["animations"];
        if (animationsToken == null)
            throw new InvalidOperationException("No 'animations' section in JSON");

        // Обходим каждую анимацию внутри "animations"
        foreach (var token in animationsToken.Children<JProperty>())
        {
            string animationKey = token.Name;
            var animationJson = token.Value as JObject;

            if (animationJson == null)
                continue;

            var animation = new Animation
            {
                Name = animationKey,
                FrameDuration = (float)animationJson["frameDuration"]!.ToObject<double>(),
                IsLooped = (bool)(animationJson["isLooped"] ?? true),
                Frames = new List<Vector2>()
            };

            // Загружаем кадры
            var framesToken = animationJson["frames"];
            if (framesToken is JArray framesArray)
            {
                foreach (var frameToken in framesArray)
                {
                    int x = (int)frameToken["x"]!;
                    int y = (int)frameToken["y"]!;
                    animation.Frames.Add(new Vector2(x, y));
                }
            }
            animationSet.Add(animationKey, animation);
        }
        return animationSet;
    }
}