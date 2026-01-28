using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LemonicLib.Input;
using System.Security.Cryptography.X509Certificates;

namespace LemonicLib;

public class Core : Game
{
    private static Core _instance;
    public static Core Instance => _instance;

    public static GraphicsDeviceManager Graphics;
    public static new GraphicsDevice GraphicsDevice;
    public static SpriteBatch SpriteBatch;
    public static new ContentManager Content;

    public static InputManager Input;

    public static bool ExitOnEscape;
    private float _exitTime;

    public Core(string title, int width, int height, bool isFullscreen)
    {  
        if (_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        _instance = this;

        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = isFullscreen;
        Graphics.ApplyChanges();

        Content = base.Content;
        Content.RootDirectory = "Content";

        Window.Title = title;

        IsMouseVisible = false;
        ExitOnEscape = true;

        _exitTime = 0;
    }

    protected override void Initialize()
    {
        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Input = new InputManager();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);

        if (Input.Keyboard.IsKeyDown(Keys.Escape))
        {
            _exitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_exitTime >= 0.5f)
            {
                Exit();
            }
        }
        else if (Input.Keyboard.IsKeyReleased(Keys.Escape))
        {
            _exitTime = 0;
        }

        base.Update(gameTime);
    }
}