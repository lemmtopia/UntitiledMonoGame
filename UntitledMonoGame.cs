using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LemonicLib;
using LemonicLib.Graphics;
using LemonicLib.Input;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

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

    private Song _linkinParkInTheEnd;
    private SoundEffect _soundEffect;

    private float _smileySpeed = 240f;
    private float _smileyX;
    private float _smileyY;

    private bool _mouseControl = false;
    private string _controlText = "Keyboard/GamePad Control";

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
        _linkinParkInTheEnd = Content.Load<Song>("audio/linkin-park_in-the-end");
        _soundEffect = Content.Load<SoundEffect>("audio/tx0_fire1");

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

        // Play Linkin Park
        if (MediaPlayer.State != MediaState.Playing)
        {
            MediaPlayer.Play(_linkinParkInTheEnd);
            MediaPlayer.Volume = 0.6f;
        }


        _smileyWalk.Update(dt);

        GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];
        
        if (Input.Keyboard.IsKeyPressed(Keys.V))
        {
            _mouseControl = !_mouseControl;

            if (_mouseControl)
            {
                _controlText = "Mouse Control";
            }
            else
            {
                _controlText = "Keyboard/GamePad Control";
            }
        }

        bool action = false;

        if (_mouseControl)
        {
            _smileyX = Input.Mouse.X;
            _smileyY = Input.Mouse.Y;

            action = Input.Mouse.IsButtonPressed(MouseButton.Left);
        }
        else
        {
            action = Input.Keyboard.IsKeyPressed(Keys.Z) || gamePadOne.IsButtonPressed(Buttons.A);

            if (gamePadOne.IsButtonPressed(Buttons.B))
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

        if (action)
        {
            _soundEffect.Play();
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
        SpriteBatch.DrawString(_font, _controlText, new Vector2(12, 8), Color.Black);

        _smileyWalk.Draw(SpriteBatch, new Rectangle((int)_smileyX, (int)_smileyY, 64, 64), Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
