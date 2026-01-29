using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LemonicLib.Input;

public class InputManager
{
    public KeyboardInfo Keyboard { get; private set; }
    public MouseInfo Mouse { get; private set; }
    public GamePadInfo[] GamePads;

    public Dictionary<string, InputAction> Actions;

    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }

        Actions = new Dictionary<string, InputAction>();
    }

    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }

    public void AddAction(string name, Keys[] keys, Buttons[] gamePadButtons, MouseButton[] mouseButtons)
    {
        Actions.Add(name, new InputAction(name, keys, gamePadButtons, mouseButtons));
    }
    
    public void RemoveAction(string name)
    {
        Actions.Remove(name);
    }

    public bool IsActionDown(PlayerIndex playerIndex, string name)
    {
        InputAction action = Actions[name];

        foreach (Keys key in action.Keys)
        {
            if (Keyboard.IsKeyDown(key)) return true;
        }

        foreach (Buttons padButton in action.GamePadButtons)
        {
            if (GamePads[(int)playerIndex].IsButtonDown(padButton)) return true;
        }

        foreach (MouseButton mosueButton in action.MouseButtons)
        {
            if (Mouse.IsButtonDown(mosueButton)) return true;
        }

        return false;
    }

    public bool IsActionUp(PlayerIndex playerIndex, string name)
    {
        InputAction action = Actions[name];

        foreach (Keys key in action.Keys)
        {
            if (Keyboard.IsKeyUp(key)) return true;
        }

        foreach (Buttons gamePadButton in action.GamePadButtons)
        {
            if (GamePads[(int)playerIndex].IsButtonUp(gamePadButton)) return true;
        }

        foreach (MouseButton mouseButton in action.MouseButtons)
        {
            if (Mouse.IsButtonUp(mouseButton)) return true;
        }

        return false;
    }

    public bool IsActionPressed(PlayerIndex playerIndex, string name)
    {
        InputAction action = Actions[name];

        foreach (Keys key in action.Keys)
        {
            if (Keyboard.IsKeyPressed(key)) return true;
        }

        foreach (Buttons gamePadButton in action.GamePadButtons)
        {
            if (GamePads[(int)playerIndex].IsButtonPressed(gamePadButton)) return true;
        }

        foreach (MouseButton mosueButton in action.MouseButtons)
        {
            if (Mouse.IsButtonPressed(mosueButton)) return true;
        }

        return false;
    }

    public bool IsActionReleased(PlayerIndex playerIndex, string name)
    {
        InputAction action = Actions[name];

        foreach (Keys key in action.Keys)
        {
            if (Keyboard.IsKeyReleased(key)) return true;
        }

        foreach (Buttons gamePadButton in action.GamePadButtons)
        {
            if (GamePads[(int)playerIndex].IsButtonReleased(gamePadButton)) return true;
        }

        foreach (MouseButton mosueButton in action.MouseButtons)
        {
            if (Mouse.IsButtonReleased(mosueButton)) return true;
        }

        return false;
    }
}