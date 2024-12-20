using System;
using UnityEngine;

/// <summary>
/// Ensures inputPreferences given are valid
/// </summary>
public class InputPreferenceValidator : MonoBehaviour
{
    private InputPreferences ip;

    /// <summary>
    /// Runs each check in sequence to ensure there are no invalid parameters.
    /// </summary>
    public void validateParameters()
    {
        ip = PlayerManager.instance.ip;

        correctSharedInconsistencies();
        configureSameInputMethod();

        for (int i = 1; i <= 2; i++)
        {
            validateKBM(i);
        }
    }

    /// <summary>
    /// Ensures sharing players have both of their control schemes set to shared and are using the same device.
    /// </summary>
    private void correctSharedInconsistencies()
    {
        // If one scheme is set to shared, set both to shared (Exclusive or)
        if ((ip.controlSchemes[0] == ControlScheme.Shared ^ ip.controlSchemes[1] == ControlScheme.Shared))
        {
            Debug.Log("One scheme was set to shared. Changed both to shared.");
            ip.controlSchemes[0] = ControlScheme.Shared; ip.controlSchemes[1] = ControlScheme.Shared;
        }

        // If the players are sharing, they should be using the same input device. Prioritise player 1's device
        if (ip.controlSchemes[0] == ControlScheme.Shared && (ip.inputMethods[0] != ip.inputMethods[1]))
        {
            Debug.Log("Scheme is set to shared yet players are using separate devices. Switch both to player 1's device.");
            ip.inputMethods[1] = ip.inputMethods[0];
        }
    }

    /// <summary>
    /// If both players are set to the same device, correct them so they use the shared control schemes if possible
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if an input device can't supplement a shared layout</exception>
    private void configureSameInputMethod()
    {
        // There are multiple special cases if the selected input methods are the same for both player and they relate to the keyboard or mouse
        if (ip.inputMethods[0] == ip.inputMethods[1])
        {
            switch (ip.inputMethods[0])
            {
                case InputMethod.Mouse:
                    throw new ArgumentException("One mouse can't be shared by two players");

                case InputMethod.KeyboardAndMouse:
                    if (ip.controlSchemes[0] != ControlScheme.Shared)
                        Debug.Log("Both players using keyboard and mouse. Changed to shared scheme.");

                    // Keyboard&Mouse layout's PlayerShared action map only detected, keyboard, not mouse. The cause is unknown.
                    // Instead, set each player to the default layout of keyboard and mouse. This has identical functionality.

                    ip.inputMethods[0] = InputMethod.Keyboard; ip.inputMethods[1] = InputMethod.Mouse;
                    ip.controlSchemes[0] = ControlScheme.Default; ip.controlSchemes[1] = ControlScheme.Default;
                    break;

                case InputMethod.Keyboard:
                    if (ip.controlSchemes[0] != ControlScheme.Shared)
                        Debug.Log("Both players using keyboard. Changed to shared scheme.");

                    ip.leftHand[0] = true; ip.leftHand[1] = false;
                    ip.controlSchemes[0] = ControlScheme.OneHand; ip.controlSchemes[1] = ControlScheme.OneHand;
                    break;
            }
        }
    }

    /// <summary>
    /// Ensures that the "Keyboard and Mouse" layout is not being used one handed.
    /// </summary>
    /// <param name="PlayerNo">Int 1, 2. Denotes Player 1 or 2</param>
    /// <exception cref="ArgumentException">Thrown if input preferences are bad</exception>
    private void validateKBM(int PlayerNo)
    {
        // You can't use a keyboard and mouse onehanded. Throw an exception.
        if (ip.inputMethods[PlayerNo - 1] == InputMethod.KeyboardAndMouse && ip.controlSchemes[PlayerNo - 1] == ControlScheme.OneHand)
            throw new ArgumentException("Cannot use KeyboardAndMouse one handed. Set device to 'Keyboard' or 'Mouse' ");
    }
}
