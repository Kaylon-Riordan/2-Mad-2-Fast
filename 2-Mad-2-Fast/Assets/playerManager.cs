using Cinemachine;
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


    public enum ControlScheme
    {
        Default,
        OneHandL,
        OneHandR,
        Shared
    }

    [SerializeField]
    public List<ControlScheme> controlSchemes;


    private void Awake()
    {
        // If one is set to shared, they both must be shared.
        if (controlSchemes[0] == ControlScheme.Shared || controlSchemes[1] == ControlScheme.Shared)
        {
            controlSchemes[0] = ControlScheme.Shared;
            controlSchemes[1] = ControlScheme.Shared;
        }

        instantiatePlayer(1);
        instantiatePlayer(2);
    }

    private void instantiatePlayer(int PlayerNo)
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        // Set default control scheme to current input device
        string controlSchemeName = InputSystem.devices[PlayerNo - 1] is Mouse && controlSchemes[PlayerNo - 1] == ControlScheme.Default ? "Keyboard&Mouse" : "Gamepad";


        // Otherwise set control scheme
        if (controlSchemes[PlayerNo - 1] != ControlScheme.Default)
            controlSchemeName = controlSchemes[PlayerNo - 1].ToString();

        if (controlSchemes[PlayerNo - 1] == ControlScheme.Shared) // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
            players[PlayerNo - 1] = PlayerInput.Instantiate(playerPrefabs[PlayerNo - 1], pairWithDevice: Gamepad.current);
        else 
            players[PlayerNo - 1] = PlayerInput.Instantiate(playerPrefabs[PlayerNo - 1], controlScheme: controlSchemeName, pairWithDevice: InputSystem.devices[PlayerNo - 1]);
    }
}
