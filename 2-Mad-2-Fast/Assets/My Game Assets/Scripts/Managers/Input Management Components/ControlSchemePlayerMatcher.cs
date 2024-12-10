using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSchemePlayerMatcher : MonoBehaviour
{
    private InputPreferences ip;

    public string getControlSchemeByPlayerNumber(int PlayerNo)
    {
        ip = PlayerManager.instance.ip;

        ControlScheme controlScheme = ip.controlSchemes[PlayerNo - 1];
        InputMethod inputMethod = ip.inputMethods[PlayerNo - 1];

        if (controlScheme == ControlScheme.OneHand)
        {
            if (inputMethod == InputMethod.Mouse)
            {
                return "Mouse"; // Mouse's Default scheme is identical to its onehanded scheme
            }

            string hand = ip.leftHand[PlayerNo - 1] ? "L" : "R";
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
