using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Matches a connected input device to a player's preference
/// </summary>
public class InputDevicePlayerMatcher : MonoBehaviour
{
    private static InputDeviceGetter deviceGetter;
    private static InputDeviceChecker checker;
    private InputPreferences ip;

    private void Awake()
    {
        deviceGetter = GetComponent<InputDeviceGetter>();
        checker = GetComponent<InputDeviceChecker>();
    }

    /// <summary>
    /// Matches input device by player number and input preference
    /// </summary>
    /// <param name="PlayerNo">Int 1 or 2 representing Player 1 or 2</param>
    /// <returns>the connected device matching the player's preferences</returns>
    public InputDevice getInputDeviceByPlayerNumber(int PlayerNo)
    {
        ip = PlayerManager.instance.ip;

        List<InputDevice> gamepads = deviceGetter.getGamepads();
        InputDevice keyboard = deviceGetter.getKeyboard();
        InputDevice mouse = deviceGetter.getMouse();

        switch (ip.inputMethods[PlayerNo - 1])
        {
            case InputMethod.KeyboardAndMouse:
                checker.checkInputDeviceNull(mouse, keyboard);
                return keyboard; // Only returning one of keyboard/mouse devices should be necessary for the default and shared schemes

            case InputMethod.Mouse:
                checker.checkInputDeviceNull(mouse);
                return mouse;

            case InputMethod.Keyboard:
                checker.checkInputDeviceNull(keyboard);
                return keyboard;
            case InputMethod.Gamepad:

                // Both players are using separate gamepads.
                if ((ip.inputMethods[0] == InputMethod.Gamepad && ip.inputMethods[1] == InputMethod.Gamepad) && ip.controlSchemes[PlayerNo - 1] != ControlScheme.Shared)
                {
                    checker.checkInputDeviceNull(gamepads, 2);
                    return gamepads[PlayerNo - 1]; // Matching gamepad to player number
                }

                else // Called if scheme is default or players are sharing one gamepad
                {
                    checker.checkInputDeviceNull(gamepads, 1);
                    return gamepads[0]; // Always return the same gamepad. If shared, both players use the same controller.
                }

            default:
                Debug.Log("Couldn't link an input device");
                return null;
        }
    }
}
