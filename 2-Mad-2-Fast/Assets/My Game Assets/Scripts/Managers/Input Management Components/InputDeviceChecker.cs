using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceChecker : MonoBehaviour
{
    // Check if an input device was returned or not
    public void checkInputDeviceNull(InputDevice device)
    {
        if (device == null) // device isnt connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    public void checkInputDeviceNull(InputDevice device1, InputDevice device2)
    {
        if (device1 == null || device2 == null) // neither device is connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    public void checkInputDeviceNull(List<InputDevice> devices, int expectedAmount)
    {
        if (devices.Count < expectedAmount) // not enough devices connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }
}
