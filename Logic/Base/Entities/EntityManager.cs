using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Entities;

internal static class EntityManager
{
    private static readonly string _folderPath = Path.Combine(AppContext.BaseDirectory, "Templates", "Entities");
    private static Dictionary<string, EntityTemplate> _templates = new();

    public static Entity Create(Vector2 position, string enemyName)
    {
        return Create(_templates[enemyName].Sprite, position, 100, enemyName);
    }

    public static Entity Create(Sprite sprite, Vector2 position, int layer, string enemyName)
    {
        if (!_templates.TryGetValue(enemyName, out var template))
            throw new ArgumentException($"Enemy type '{enemyName}' not found");

        return enemyName switch
        {
            "skeleton" => new Skeleton(sprite, position, layer, template),
            "player" => new Player(sprite, position, layer, template),
            _ => throw new NotImplementedException($"No enemy class implemented for type '{enemyName}'")
        };
    }

    public static void LoadAllTemplates(ContentManager content)
    {
        foreach (var filePath in Directory.GetFiles(_folderPath, $"*.json"))
        {
            var template = LoadTemplate(filePath, content);
            if (!_templates.ContainsKey(template.Name))
                _templates.Add(template.Name, template);
        }
    }

    private static EntityTemplate LoadTemplate(string fileName, ContentManager content)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Entity template file not found: {fileName}");

        string jsonText = File.ReadAllText(fileName);
        JObject jsonData = JObject.Parse(jsonText);

        EntityTemplate template = new();
        template = JsonConvert.DeserializeObject<EntityTemplate>(jsonText);
        template.Sprite = new(template.SpriteName, content);

        template.AnimationSet = AnimationManager.GetAnimationSet(template.Name + "_anim");

        return template;
    }
}
