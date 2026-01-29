using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LemonicLib.Input;

public class InputAction
{
    public string Name;

    public Keys[] Keys;
    public Buttons[] GamePadButtons;
    public MouseButton[] MouseButtons;

    public InputAction(string name, Keys[] keys, Buttons[] gamePadButtons, MouseButton[] mouseButtons)
    {
        Name = name;
        Keys = keys;
        GamePadButtons = gamePadButtons;
        MouseButtons = mouseButtons;
    }
}