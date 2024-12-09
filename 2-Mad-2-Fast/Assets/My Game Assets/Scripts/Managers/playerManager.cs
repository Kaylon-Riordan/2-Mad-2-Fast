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

    public static PlayerManager instance;

    private static InputPreferenceValidator validator;
    public InputPreferences ip;

    [SerializeField]
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<GameObject> playerPrefabs = new List<GameObject>();

    private void Awake()
    {
        instance = Singleton<PlayerManager>.get();
        validator = GetComponent<InputPreferenceValidator>();
    }

    private void Start()
    {
        // Ensure all input schemes are valid
        ip = validator.validateParameters();

        instantiatePlayer(1); instantiatePlayer(2);
    }

    private void instantiatePlayer(int PlayerNo)
    {
        InputDevice device = GetComponent<InputDevicePlayerMatcher>().getInputDeviceByPlayerNumber(PlayerNo);

        // If the control scheme is set to default or onehanded for a player, determine whether it's keyboard or mouse
        string controlSchemeName = GetComponent<ControlSchemePlayerMatcher>().getControlSchemeByPlayerNumber(PlayerNo);

        // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
        players[PlayerNo - 1] = PlayerInput.Instantiate(playerPrefabs[PlayerNo - 1], controlScheme: controlSchemeName, pairWithDevice: device);

        players[PlayerNo-1].gameObject.transform.position = new Vector3(-30, 5, 45);
    }
}
