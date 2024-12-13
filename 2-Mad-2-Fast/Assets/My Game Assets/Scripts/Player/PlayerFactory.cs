using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFactory : MonoBehaviour
{
    public GameObject instantiatePlayer(int PlayerNo, string controlSchemeName, InputDevice device)
    {
        GameObject player;

        // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
        switch (PlayerNo)
        {
            case 1:
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
