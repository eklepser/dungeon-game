using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Venefica.Logic.Base;

namespace Venefica.Logic.UI;

internal abstract class UiObject
{
    public bool IsEnabled;
    public float Alpha;

    public abstract void Update(Vector2 cursorPosition, Player player, float deltaTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}
