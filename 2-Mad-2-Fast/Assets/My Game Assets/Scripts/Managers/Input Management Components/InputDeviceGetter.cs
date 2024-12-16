using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gets various types of input devices
/// </summary>
public class InputDeviceGetter : MonoBehaviour
{
    /// <summary>
    /// Get a list of gamepads connected
    /// </summary>
    /// <returns>A list of connected gamepads</returns>
    public List<InputDevice> getGamepads()
    {
        List<InputDevice> gamepads = new List<InputDevice>();

        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
                gamepads.Add(device);
        }

        return gamepads;
    }

    /// <summary>
    /// Get mouse connected
    /// </summary>
    /// <returns>Mouse input device</returns>
    public InputDevice getMouse()
    {
        InputDevice mouse = null;

        foreach (var device in InputSystem.devices)
        {
            if (device is Mouse)
                mouse = device;
        }

        return mouse;
    }

    /// <summary>
    /// Get keyboard connected
    /// </summary>
    /// <returns>Keyboard input device</returns>
    public InputDevice getKeyboard()
    {
        InputDevice keyboard = null;

        foreach (var device in InputSystem.devices)
        {
            if (device is Keyboard)
                keyboard = device;
        }

        return keyboard;
    }
}
