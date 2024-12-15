using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Initialises the desired player according to the Factory Design pattern
/// </summary>
public class PlayerFactory : MonoBehaviour
{
    /// <summary>
    /// Instantiates a player into the scene
    /// </summary>
    /// <param name="PlayerNo">An int of value 1 or 2 denoting whether it's player 1 or player 2 to be spawned</param>
    /// <param name="controlSchemeName">The name of the control scheme used by the updated Input Manager</param>
    /// <param name="device">The input device that will control the instantiated player</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public GameObject instantiatePlayer(int PlayerNo, string controlSchemeName, InputDevice device)
    {
        GameObject player;

        // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
        switch (PlayerNo)
        {
            case 1:
                // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Resources.Load.html
                player = PlayerInput.Instantiate(Resources.Load<GameObject>("Prefabs/Player 1"), controlScheme: controlSchemeName, pairWithDevice: device).gameObject;
                return player;
            case 2:
                player = PlayerInput.Instantiate(Resources.Load<GameObject>("Prefabs/Player 2"), controlScheme: controlSchemeName, pairWithDevice: device).gameObject;
                return player;
            default:
                throw new ArgumentException("Invalid player number given: " + PlayerNo);
        }
    }
}
