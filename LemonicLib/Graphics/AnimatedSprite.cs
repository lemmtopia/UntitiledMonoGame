using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LemonicLib.Graphics;

public class AnimatedSprite : Sprite
{
    public int Rows { get; set; }
    public int Columns { get; set; }

    private int _currentFrame;
    private int _totalFrames;

    public int CurrentFrame => _currentFrame;
    public int TotalFrames => _totalFrames;

    public float AnimationSpeed;
    private float _currentTime;

    public AnimatedSprite() : base()
    {
        
    }

    public AnimatedSprite(Texture2D texture, int rows, int columns, float animationSpeed) : base(texture)
    {
        Rows = rows;
        Columns = columns;

        AnimationSpeed = animationSpeed;

        _currentTime = 0;
        _currentFrame = 0;
        _totalFrames = Rows * Columns;
    }

    public void Update(float dt)
    {
        _currentTime += AnimationSpeed * dt;
        if (_currentTime >= 1)
        {
            _currentFrame++;
            if (_currentFrame >= _totalFrames)
            {
                // TODO (lemmtopia): Toggle animation loop based on a variable
                _currentFrame = 0;
            }

            _currentTime = 0;
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
    {
        int frameW = Width / Columns;
        int frameH = Height / Rows;

        int column = _currentFrame % Columns;
        int row = _currentFrame / Columns;

        Rectangle sourceRect = new Rectangle(column * frameW, row * frameH, frameW, frameH);

        spriteBatch.Draw(Texture, destination, sourceRect, color);
    }

    public static AnimatedSprite FromFile(ContentManager content, string fileName)
    {
        AnimatedSprite sprite = new AnimatedSprite();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                string texturePath = root.Element("Texture").Value;
                sprite.Texture = content.Load<Texture2D>(texturePath);

                sprite.Columns = int.Parse(root.Element("Columns").Value);
                sprite.Rows = int.Parse(root.Element("Rows").Value);
                sprite.AnimationSpeed = float.Parse(root.Element("AnimationSpeed").Value);

                sprite._currentTime = 0;
                sprite._currentFrame = 0;
                sprite._totalFrames = sprite.Rows * sprite.Columns;
            }
        }

        return sprite;
    }
}