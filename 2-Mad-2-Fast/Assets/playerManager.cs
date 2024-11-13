using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

// https://github.com/onewheelstudio/Adventures-in-C-Sharp/blob/main/Split%20Screen/PlayerManager.cs
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<GameObject> playerPrefabs = new List<GameObject>();
    [SerializeField]
    private List<Transform> spawnPoints;

    private PlayerInputManager playerInputManager;

    [SerializeField]
    private List<InputDevice> inputDevices = new List<InputDevice>();

    [SerializeField] // Determies which side will be chosen for one handed control scheme
    private bool leftHand = true;

    public enum InputMethod
    {
        Keyboard,
        Mouse,
        KeyboardAndMouse,
        Gamepad
    }

    [SerializeField]
    private List<InputMethod> selectedInputMethods;

    public enum ControlScheme
    {
        Default,
        OneHand,
        Shared
    }

    [SerializeField]
    public List<ControlScheme> controlSchemes;

    private void Awake()
    {
        instantiatePlayer(1);
        instantiatePlayer(2);
    }

    private List<InputDevice> getGamepads()
    {
        List<InputDevice> gamepads = new List<InputDevice>();

        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
                gamepads.Add(device);
        }

        return gamepads;
    }

    private InputDevice getMouse()
    {
        InputDevice mouse = null;

        foreach (var device in InputSystem.devices)
        {
            if (device is Mouse)
                mouse = device;
        }

        return mouse;
    }

    private InputDevice getKeyboard()
    {
        InputDevice keyboard = null;

        foreach (var device in InputSystem.devices)
        {
            if (device is Keyboard)
                keyboard = device;
        }

        return keyboard;
    }

    // Ensures that input devices arguments are set correctly. Either corrects them or throws an exception if they aren't
    private void validateParameters(int PlayerNo, InputMethod selectedInputMethod)
    {
        // If one scheme is set to shared, they both must be shared.
        if (controlSchemes[0] == ControlScheme.Shared || controlSchemes[1] == ControlScheme.Shared)
        {
            Debug.Log("One scheme was set to shared. Changed both to shared.");
            controlSchemes[0] = ControlScheme.Shared;
            controlSchemes[1] = ControlScheme.Shared;
        }

        // Both players are set to mouse and keyboard. We can assume both a keyboard and mouse is connected and set both players to share
        if (selectedInputMethods[0] == InputMethod.KeyboardAndMouse && selectedInputMethods[1] == InputMethod.KeyboardAndMouse)
        {
            Debug.Log("Both players using keyboard and mouse. Changed to shared scheme.");
            controlSchemes[0] = ControlScheme.Shared; controlSchemes[1] = ControlScheme.Shared;
        }

        // You can't use a keyboard and mouse onehanded. Throw an exception.
        if (selectedInputMethod == InputMethod.KeyboardAndMouse && controlSchemes[PlayerNo - 1] == ControlScheme.OneHand)
            throw new ArgumentException("Cannot use KeyboardAndMouse one handed. Set device to 'Keyboard' or 'Mouse' ");

        // You can't share the mouse on its own or the keyboard on its own. Throw an exception.
        if (selectedInputMethods[0] == InputMethod.Keyboard && selectedInputMethods[1] == InputMethod.Keyboard
            || selectedInputMethods[0] == InputMethod.Mouse && selectedInputMethods[1] == InputMethod.Mouse)
            throw new ArgumentException("Two players are set to the same input device which can't be shared");
    }

    // Check if an input device was returned or not
    private void checkInputDeviceNull(InputDevice device)
    {
        if (device == null) // device isnt connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    private void checkInputDeviceNull(InputDevice device1, InputDevice device2)
    {
        if (device1 == null || device2 == null) // neither device is connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    private void checkInputDeviceNull(List<InputDevice> devices, int expectedAmount)
    {
        if (devices.Count < expectedAmount) // not enough devices connected
            throw new ArgumentException("Arguments don't match controllers connected");
    }

    private InputDevice getInputDevice(int PlayerNo, InputMethod selectedInputMethod)
    {
        validateParameters(PlayerNo, selectedInputMethod);

        List<InputDevice> gamepads = getGamepads();
        InputDevice keyboard = getKeyboard();
        InputDevice mouse = getMouse();

        switch(selectedInputMethod)
        {
            case InputMethod.KeyboardAndMouse:
                checkInputDeviceNull(mouse, keyboard);
                return keyboard; // Only returning one of keyboard/mouse devices should be necessary for the default and shared schemes

            case InputMethod.Mouse:
                checkInputDeviceNull(mouse);
                return mouse;

            case InputMethod.Keyboard:
                checkInputDeviceNull(keyboard);
                return keyboard;
            case InputMethod.Gamepad:

                // Both players are using separate gamepads.
                if ((selectedInputMethods[0] == InputMethod.Gamepad && selectedInputMethods[1] == InputMethod.Gamepad) && controlSchemes[PlayerNo - 1] != ControlScheme.Shared)
                {
                    checkInputDeviceNull(gamepads, 2);
                    return gamepads[PlayerNo - 1]; // Matching gamepad to player number
                }

                else // Called if scheme is default or players are sharing one gamepad
                {
                    checkInputDeviceNull(gamepads, 1);
                    return gamepads[0]; // Always return the same gamepad. If shared, both players use the same controller.
                }
            default:
                Debug.Log("Couldn't link an input device");
                return null;
        }
    }

    private string getControlSchemeName(ControlScheme controlScheme, InputMethod inputMethod)
    {

        if (controlScheme == ControlScheme.OneHand)
        {
            if(inputMethod == InputMethod.Mouse)
            {
                return "Mouse"; // Mouse's Default scheme is identical to its onehanded scheme
            }

            string hand = leftHand ? "L" : "R";
            string isKeyboard = inputMethod == InputMethod.Keyboard || inputMethod == InputMethod.Mouse ? "K" : "";

            return "OneHand"+ hand + isKeyboard;
            // It's not possible to use Keyboard&Mouse one handed. An exception should be thrown above if this is attempted.
        }
        
        if (controlScheme == ControlScheme.Default || controlScheme == ControlScheme.Shared) // Default & Shared Scheme
        {
            if (inputMethod == InputMethod.Gamepad)             return "Gamepad";
            if (inputMethod == InputMethod.Mouse)               return "Mouse"; 
            if (inputMethod == InputMethod.Keyboard)            return "Keyboard";
            if (inputMethod == InputMethod.KeyboardAndMouse)    return "Keyboard&Mouse";
        }

        Debug.Log("Couldn't assign a control scheme, ControlScheme: " + controlScheme + " inputMethod: " + inputMethod);
        return null;
    }


    private void instantiatePlayer(int PlayerNo)
    {
        InputDevice device = getInputDevice(PlayerNo, selectedInputMethods[PlayerNo - 1]);

        // If the control scheme is set to default or onehanded for a player, determine whether it's keyboard or mouse
        string controlSchemeName = getControlSchemeName(controlSchemes[PlayerNo - 1], selectedInputMethods[PlayerNo - 1]);

        // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
        players[PlayerNo - 1] = PlayerInput.Instantiate(playerPrefabs[PlayerNo - 1], controlScheme: controlSchemeName, pairWithDevice: device);
    }
}
