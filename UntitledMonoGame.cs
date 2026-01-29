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

        Input.AddAction("Left", [ Keys.Left, Keys.A ], [ Buttons.DPadLeft ], []);
        Input.AddAction("Right", [ Keys.Right, Keys.D ], [ Buttons.DPadRight ], []);
        Input.AddAction("Up", [ Keys.Up, Keys.W ], [ Buttons.DPadUp ], []);
        Input.AddAction("Down", [ Keys.Down, Keys.S ], [ Buttons.DPadDown ], []);
        Input.AddAction("Fire", [ Keys.Space ], [ Buttons.A ], []);
        Input.AddAction("MouseFire", [], [], [ MouseButton.Left ]);
        Input.AddAction("GamePadVibrationTest", [], [ Buttons.X ], []);
        Input.AddAction("ToggleMouseControl", [ Keys.V ], [], []);
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
        
        if (Input.IsActionPressed(PlayerIndex.One, "ToggleMouseControl"))
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

        if (_mouseControl)
        {
            _smileyX = Input.Mouse.X;
            _smileyY = Input.Mouse.Y;

            if (Input.IsActionPressed(PlayerIndex.One, "MouseFire"))
            {
                _soundEffect.Play();
            }
        }
        else
        {
            if (Input.IsActionPressed(PlayerIndex.One, "GamePadVibrationTest"))
            {
                gamePadOne.SetVibration(0.5f, TimeSpan.FromSeconds(1));
            }

            if (Input.IsActionPressed(PlayerIndex.One, "Fire"))
            {
                _soundEffect.Play();
            }

            if (gamePadOne.LeftThumbStick != Vector2.Zero)
            {
               _smileyX += gamePadOne.LeftThumbStick.X * _smileySpeed * dt;
               _smileyY -= gamePadOne.LeftThumbStick.Y * _smileySpeed * dt;
            }
            else 
            {
                if (Input.IsActionDown(PlayerIndex.One, "Left"))
                {
                    _smileyX -= _smileySpeed * dt;
                }
                if (Input.IsActionDown(PlayerIndex.One, "Right"))
                {
                    _smileyX += _smileySpeed * dt;
                }
                if (Input.IsActionDown(PlayerIndex.One, "Up"))
                {
                    _smileyY -= _smileySpeed * dt;
                }
                if (Input.IsActionDown(PlayerIndex.One, "Down"))
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
        SpriteBatch.DrawString(_font, _controlText, new Vector2(12, 8), Color.Black);

        _smileyWalk.Draw(SpriteBatch, new Rectangle((int)_smileyX, (int)_smileyY, 64, 64), Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
