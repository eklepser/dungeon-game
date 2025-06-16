using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Base;

namespace Venefica.Logic.UserInterface;

internal abstract class UserInterfaceObject
{
    public bool IsVisible { get; protected set; } = false;
    public abstract void Update(float deltaTime, Camera camera);

    public abstract void ChangeVisibility();
}





