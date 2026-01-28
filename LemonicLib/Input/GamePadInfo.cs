using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LemonicLib.Input;

public class GamePadInfo 
{
    public GamePadState CurrentState;
    public GamePadState PreviousState;

    private TimeSpan _vibrationTimeRemaining = TimeSpan.Zero;

    public PlayerIndex PlayerIndex { get; }

    public bool IsConnected => CurrentState.IsConnected;

    public Vector2 LeftThumbStick => CurrentState.ThumbSticks.Left;
    public Vector2 RightThumbStick => CurrentState.ThumbSticks.Right;

    public float LeftTrigger => CurrentState.Triggers.Left;
    public float RightTrigger => CurrentState.Triggers.Right;

    public GamePadInfo(PlayerIndex playerIndex)
    {
        PlayerIndex = playerIndex;

        PreviousState = new GamePadState();
        CurrentState = GamePad.GetState(PlayerIndex);
    }

    public void Update(GameTime gameTime)
    {
        PreviousState = CurrentState;
        CurrentState = GamePad.GetState(PlayerIndex);

        if (_vibrationTimeRemaining > TimeSpan.Zero)
        {
            _vibrationTimeRemaining -= gameTime.ElapsedGameTime;

            if (_vibrationTimeRemaining <= TimeSpan.Zero)
            {
                StopVibration();
            }
        }
    }

    public bool IsButtonDown(Buttons button)
    {
        return CurrentState.IsButtonDown(button);
    }
    
    public bool IsButtonUp(Buttons button)
    {
        return CurrentState.IsButtonUp(button);
    }

    public bool IsButtonPressed(Buttons button)
    {
        return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
    }

    public bool IsButtonReleased(Buttons button)
    {
        return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
    }

    public void SetVibration(float strength, TimeSpan time)
    {
        GamePad.SetVibration(PlayerIndex, strength, strength);
        _vibrationTimeRemaining = time;
    }

    public void StopVibration()
    {
        GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
    }
}