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

    // https://github.com/Ben-Keev/Tower/blob/main/Assets/Scripts/GameManager.cs.cs
    public static PlayerManager instance;

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

    [SerializeField]
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<GameObject> playerPrefabs = new List<GameObject>();

    private void Awake()
    {
        initSingleton();
        instantiatePlayer(1);
        instantiatePlayer(2);
    }

    private void instantiatePlayer(int PlayerNo)
    {
        InputDevice device = InputManager.instance.getInputDevice(PlayerNo);

        // If the control scheme is set to default or onehanded for a player, determine whether it's keyboard or mouse
        string controlSchemeName = InputManager.instance.getControlSchemeName(PlayerNo);

        Debug.Log(PlayerNo + ": " + controlSchemeName);

        // https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
        players[PlayerNo - 1] = PlayerInput.Instantiate(playerPrefabs[PlayerNo - 1], controlScheme: controlSchemeName, pairWithDevice: device);
    }
}
