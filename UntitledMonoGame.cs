using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LemonicLib;
using LemonicLib.Graphics;
using LemonicLib.Input;
using System;

namespace UntitiledMonoGame;

public class UntitledMonoGame : Core
{
    private SpriteFont _font;

    private Texture2D _backgroundTexture;
    private Sprite _background;
    
    private Texture2D _tilesetTexture;
    private SpriteRegion _tileset;

    private Texture2D _smileyWalkTetxure;
    AnimatedSprite _smileyWalk;

    private float _smileySpeed = 240f;
    private float _smileyX;
    private float _smileyY;

    private bool _mouseControl = false;

    public UntitledMonoGame() : base("Untitled", 800, 480, false)
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

        _smileyX = 400;
        _smileyY = 300;
    }

    protected override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("message_font");

        _tilesetTexture = Content.Load<Texture2D>("textures/tileset");
        _tileset = new SpriteRegion(_tilesetTexture, new Rectangle(128, 0, 32, 32));

        _backgroundTexture = Content.Load<Texture2D>("textures/background");
        _background = new Sprite(_backgroundTexture);

        _smileyWalkTetxure = Content.Load<Texture2D>("textures/SmileyWalk");
        _smileyWalk = new AnimatedSprite(_smileyWalkTetxure, 4, 4, 8f);
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _smileyWalk.Update(dt);

        GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];
        
        if (Input.Keyboard.IsKeyPressed(Keys.V))
        {
            _mouseControl = !_mouseControl;
        }

        if (_mouseControl)
        {
            _smileyX = Input.Mouse.X;
            _smileyY = Input.Mouse.Y;
        }
        else
        {
            if (gamePadOne.IsButtonPressed(Buttons.A))
            {
                gamePadOne.SetVibration(0.5f, TimeSpan.FromSeconds(1));
            }

            if (gamePadOne.LeftThumbStick != Vector2.Zero)
            {
               _smileyX += gamePadOne.LeftThumbStick.X * _smileySpeed * dt;
               _smileyY -= gamePadOne.LeftThumbStick.Y * _smileySpeed * dt;
            }
            else 
            {
                bool left = Input.Keyboard.IsKeyDown(Keys.Left) || gamePadOne.IsButtonDown(Buttons.DPadLeft);
                bool right = Input.Keyboard.IsKeyDown(Keys.Right) || gamePadOne.IsButtonDown(Buttons.DPadRight);
                bool up = Input.Keyboard.IsKeyDown(Keys.Up) || gamePadOne.IsButtonDown(Buttons.DPadUp);
                bool down = Input.Keyboard.IsKeyDown(Keys.Down) || gamePadOne.IsButtonDown(Buttons.DPadDown);

                if (left)
                {
                    _smileyX -= _smileySpeed * dt;
                }
                if (right)
                {
                    _smileyX += _smileySpeed * dt;
                }
                if (up)
                {
                    _smileyY -= _smileySpeed * dt;
                }
                if (down)
                {
                    _smileyY += _smileySpeed * dt;
                }
            }
        }

        _smileyX = MathHelper.Clamp(_smileyX, 0, 800 - 64);
        _smileyY = MathHelper.Clamp(_smileyY, 0, 480 - 64);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _background.Draw(SpriteBatch, new Rectangle(0, 0, 800, 480), Color.White);
        _tileset.Draw(SpriteBatch, new Rectangle(64, 64, 32, 32), Color.White);
        SpriteBatch.DrawString(_font, "Hello, world", new Vector2(12, 8), Color.Black);

        _smileyWalk.Draw(SpriteBatch, new Rectangle((int)_smileyX, (int)_smileyY, 64, 64), Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
