using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Venefica.Logic.Graphics;

internal class Animation
{
    public List<Vector2> Frames { get; set; } = new();
    public float FrameDuration { get; set; } // время одного кадра в секундах
    public bool IsLooped { get; set; }
    public string Name { get; set; }
}
