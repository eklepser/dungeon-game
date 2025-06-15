using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Venefica.Logic.Graphics;

internal class Animation
{
    public string Name { get; set; }
    public List<Vector2> Frames { get; set; } = new();
    public float FrameDuration { get; set; }
    public bool IsLooped { get; set; } 
}
