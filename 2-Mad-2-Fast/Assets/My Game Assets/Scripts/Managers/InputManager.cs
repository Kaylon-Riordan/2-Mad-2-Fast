using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // https://github.com/Ben-Keev/Tower/blob/main/Assets/Scripts/GameManager.cs.cs
    public static InputManager instance;

    private void initSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    [SerializeField] // Determines which side will be chosen for one handed control scheme
    private List<bool> leftHand = new List<bool>();

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
        initSingleton();
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
        // If one scheme is set to shared, set both to shared (Exclusive or)
        if ((controlSchemes[0] == ControlScheme.Shared ^ controlSchemes[1] == ControlScheme.Shared))
        {
            Debug.Log("One scheme was set to shared. Changed both to shared.");
            controlSchemes[0] = ControlScheme.Shared;  controlSchemes[1] = ControlScheme.Shared;
        }

        // If the players are sharing, they should be using the same input device. Prioritise player 1's device
        if (controlSchemes[0] == ControlScheme.Shared && (selectedInputMethods[0] != selectedInputMethods[1]))
        {
            Debug.Log("Scheme is set to shared yet players are using separate devices. Switch both to player 1's device.");
            selectedInputMethods[1] = selectedInputMethods[0];
        }

        // You can't use a keyboard and mouse onehanded. Throw an exception.
        if (selectedInputMethod == InputMethod.KeyboardAndMouse && controlSchemes[PlayerNo - 1] == ControlScheme.OneHand)
            throw new ArgumentException("Cannot use KeyboardAndMouse one handed. Set device to 'Keyboard' or 'Mouse' ");

        // There are multiple special cases if the selected input methods are the same for both player and they relate to the keyboard or mouse
        if (selectedInputMethods[0] == selectedInputMethods[1])
        {
            switch(selectedInputMethods[0])
            {
                case InputMethod.Mouse:
                    throw new ArgumentException("One mouse can't be shared by two players");

                case InputMethod.KeyboardAndMouse:
                    if (controlSchemes[0] != ControlScheme.Shared)
                        Debug.Log("Both players using keyboard and mouse. Changed to shared scheme.");

                    // Keyboard&Mouse layout's PlayerShared action map only detected, keyboard, not mouse. The cause is unknown.
                    // Instead, set each player to the default layout of keyboard and mouse. This has identical functionality.

                    selectedInputMethods[0] = InputMethod.Keyboard; selectedInputMethods[1] = InputMethod.Mouse;
                    controlSchemes[0] = ControlScheme.Default; controlSchemes[1] = ControlScheme.Default;
                    break;

                case InputMethod.Keyboard:
                    if (controlSchemes[0] != ControlScheme.Shared)
                        Debug.Log("Both players using keyboard. Changed to shared scheme.");

                    leftHand[0] = true; leftHand[1] = false;
                    controlSchemes[0] = ControlScheme.OneHand; controlSchemes[1] = ControlScheme.OneHand;
                    break;
                }
        }
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

    public InputDevice getInputDevice(int PlayerNo)
    {
        InputMethod selectedInputMethod = selectedInputMethods[PlayerNo - 1];
        validateParameters(PlayerNo, selectedInputMethod);

        List<InputDevice> gamepads = getGamepads();
        InputDevice keyboard = getKeyboard();
        InputDevice mouse = getMouse();

        switch (selectedInputMethod)
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

    public string getControlSchemeName(int PlayerNo)
    {
        ControlScheme controlScheme = controlSchemes[PlayerNo - 1];
        InputMethod inputMethod = selectedInputMethods[PlayerNo - 1];

        if (controlScheme == ControlScheme.OneHand)
        {
            if (inputMethod == InputMethod.Mouse)
            {
                return "Mouse"; // Mouse's Default scheme is identical to its onehanded scheme
            }

            string hand = leftHand[PlayerNo-1] ? "L" : "R";
            string isKeyboard = inputMethod == InputMethod.Keyboard ? "K" : "";

            Debug.Log("Couldn't assign a control scheme, ControlScheme: " + "OneHand" + hand + isKeyboard
            + " inputMethod: " + inputMethod);


            return "OneHand" + hand + isKeyboard;
            // It's not possible to use Keyboard&Mouse one handed. An exception should be thrown above if this is attempted.
        }

        if (controlScheme == ControlScheme.Default || controlScheme == ControlScheme.Shared) // Default & Shared Scheme
        {
            if (inputMethod == InputMethod.Gamepad) return "Gamepad";
            if (inputMethod == InputMethod.Mouse) return "Mouse";
            if (inputMethod == InputMethod.Keyboard) return "Keyboard";
            if (inputMethod == InputMethod.KeyboardAndMouse) return "Keyboard&Mouse";
        }

        Debug.Log("Couldn't assign a control scheme, ControlScheme: " + controlScheme + " inputMethod: " + inputMethod);
        return null;
    }
}
