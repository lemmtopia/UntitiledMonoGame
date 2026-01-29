using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LemonicLib.Graphics;

public class SpriteRegion : Sprite
{
    public Rectangle Region { get; set; }

    public SpriteRegion(Texture2D texture, Rectangle region) : base (texture)
    {
        Region = region;
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
    {
        spriteBatch.Draw(Texture, destination, Region, color);
    }
}