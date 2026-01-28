using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LemonicLib.Graphics;

public class SpriteRegion : Sprite
{
    public Rectangle RegionRectangle { get; set; }

    public SpriteRegion(Texture2D texture, Rectangle regionRect) : base (texture)
    {
        RegionRectangle = regionRect;
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
    {
        spriteBatch.Draw(Texture, destination, RegionRectangle, color);
    }
}