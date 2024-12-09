using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceGetter : MonoBehaviour
{
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
