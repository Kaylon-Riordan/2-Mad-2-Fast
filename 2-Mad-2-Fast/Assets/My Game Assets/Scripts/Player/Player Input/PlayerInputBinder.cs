using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBinder : MonoBehaviour
{
    [HideInInspector]
    public InputActionMap playerActions;
    private InputActionAsset playerActionAsset;

    [HideInInspector]
    public InputAction steer;
    [HideInInspector]
    public InputAction leftPedal;
    [HideInInspector]
    public InputAction rightPedal;
    [HideInInspector]
    public InputAction brake;

    [SerializeField]
    public int PlayerNo;

    private void Awake()
    {
        playerActionAsset = GetComponent<PlayerInput>().actions;

        if (PlayerManager.instance.ip.controlSchemes[PlayerNo - 1] == ControlScheme.Shared)
        {
            playerActions = playerActionAsset.FindActionMap("PlayerShared");
            getSharedInputDeviceInputs();
        }
        else
        {
            playerActions = playerActionAsset.FindActionMap("Player");
            getIndependentInputDeviceInputs();
        }
    }

    // Called where each player has their own input device (more common)
    private void getIndependentInputDeviceInputs()
    {
        // Find Actions
        steer = playerActions.FindAction("Steer");
        leftPedal = playerActions.FindAction("LeftPedal");
        rightPedal = playerActions.FindAction("RightPedal");
        brake = playerActions.FindAction("Brake");
    }

    // Called where each player shares a single input device
    private void getSharedInputDeviceInputs()
    {
        // Find Actions
        steer = playerActions.FindAction("SteerP" + PlayerNo);
        leftPedal = playerActions.FindAction("LeftPedalP" + PlayerNo);
        rightPedal = playerActions.FindAction("RightPedalP" + PlayerNo);
        brake = playerActions.FindAction("BrakeP" + PlayerNo);
    }
}
