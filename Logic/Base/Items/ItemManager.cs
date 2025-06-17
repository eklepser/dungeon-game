using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using Venefica.Logic.Base.Entities;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Base.Items.Staffs;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Weapons;

internal static class ItemManager
{
    private const string _folderPath = "../../../Templates/Items";
    private static Dictionary<string, StaffTemplate> _staffTemplates = new();

    public static Item Create(string staffName)
    {
        if (!_staffTemplates.TryGetValue(staffName, out var template))
            throw new ArgumentException($"Enemy type '{staffName}' not found");

        return staffName switch
        {
            "red_staff" => new RedStaff(template),
            "blue_staff" => new BlueStaff(template),
            _ => throw new NotImplementedException($"No enemy class implemented for type '{staffName}'")
        };
    }

    public static void LoadAllStaffTemplates(ContentManager content)
    {
        if (!Directory.Exists(_folderPath))
            throw new DirectoryNotFoundException($"Staff template folder not found: {_folderPath}");

        foreach (var filePath in Directory.GetFiles(_folderPath, "*.json"))
        {
            var template = LoadStaffTemplate(filePath, content);
            if (_staffTemplates.ContainsKey(template.Name))
                continue;

            _staffTemplates.Add(template.Name, template);
        }
    }

    private static StaffTemplate LoadStaffTemplate(string fileName, ContentManager content)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Staff template file not found: {fileName}");

        string jsonText = File.ReadAllText(fileName);
        var template = JsonConvert.DeserializeObject<StaffTemplate>(jsonText);

        template.Projectile.Sprite = new(template.Projectile.SpriteName, content);

        template.Projectile.Sprite.TextureSize = 10;

        template.Sprite = new Sprite(template.SpriteName, content, 32);

        return template;
    }
}
