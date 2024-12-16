using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contains methods that check whether an input device is null or not.
/// </summary>
public class InputDeviceChecker : MonoBehaviour
{
    /// <summary>
    /// Check if a device is connected
    /// </summary>
    /// <param name="device">Desired device</param>
    /// <exception cref="ArgumentException">Thrown if device isn't connected</exception>
    public void checkInputDeviceNull(InputDevice device)
    {
        if (device == null) // device isnt connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    /// <summary>
    /// Checks if two devices are connected
    /// </summary>
    /// <param name="device1">The first desired device</param>
    /// <param name="device2">The second desired device</param>
    /// <exception cref="ArgumentException">Thrown if the first OR second device wasn't connected</exception>
    public void checkInputDeviceNull(InputDevice device1, InputDevice device2)
    {
        if (device1 == null || device2 == null) // neither device is connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    /// <summary>
    /// Checks whether a list of devices matches an expected amount of devices
    /// </summary>
    /// <param name="devices">A list containing each desired device</param>
    /// <param name="expectedAmount">How many devices are expected</param>
    /// <exception cref="ArgumentException">Thrown if the expected devices does not match actual devices</exception>
    public void checkInputDeviceNull(List<InputDevice> devices, int expectedAmount)
    {
        if (devices.Count < expectedAmount) // not enough devices connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }
}
