using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LemonicLib.Graphics;

public class Sprite
{
    public Texture2D Texture { get; set; }

    public int Width => Texture.Width;
    public int Height => Texture.Height;

    public Sprite(Texture2D texture)
    {
        Texture = texture;
    }

    public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
    {
        spriteBatch.Draw(Texture, destination, new Rectangle(0, 0, Width, Height), color);
    }
}