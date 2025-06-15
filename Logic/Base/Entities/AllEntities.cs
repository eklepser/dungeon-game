using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.Base.Entities;

internal class Skeleton : Entity
{
    public Skeleton(Sprite sprite, Vector2 position, int layer, EntityTemplate template) : base(sprite, position, layer, template) { }
}
