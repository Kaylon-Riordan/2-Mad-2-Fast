using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputHandler : MonoBehaviour
{
    private InputActionAsset playerActionAsset;
    private InputActionMap playerActions;

    private InputAction steer;
    private InputAction leftPedal;
    private InputAction rightPedal;
    private InputAction brake;

    private playerController controller;

    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private int playerNumber;

    private void Awake()
    {
        controller = GetComponent<playerController>();
        playerActionAsset = GetComponent<PlayerInput>().actions;

        // Find the playerManager singleton gameObject and get its component
        playerManager = GameObject.FindGameObjectsWithTag("playerManager")[0].GetComponent<PlayerManager>();

        if (playerManager.controlSchemes[playerNumber-1] == PlayerManager.ControlScheme.Shared)
            playerActions = playerActionAsset.FindActionMap("PlayerShared");
        else
            playerActions = playerActionAsset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        if (playerManager.controlSchemes[playerNumber-1] == PlayerManager.ControlScheme.Shared)
            getSharedInputDeviceInputs();
        else
            getIndependentInputDeviceInputs();

        // Execute methods if performed
        leftPedal.performed += controller.LeftPedal;
        rightPedal.performed += controller.RightPedal;
        brake.performed += controller.BrakePressed;
        brake.canceled += controller.BrakeReleased;

        playerActions.Enable();
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
        steer = playerActions.FindAction("SteerP" + playerNumber);
        leftPedal = playerActions.FindAction("LeftPedalP" + playerNumber);
        rightPedal = playerActions.FindAction("RightPedalP" + playerNumber);
        brake = playerActions.FindAction("BrakeP" + playerNumber);

        
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    public Vector2 GetSteer()
    {
        if (playerManager.controlSchemes[playerNumber - 1] == PlayerManager.ControlScheme.Shared)
            return playerActions.FindAction("SteerP" + playerNumber).ReadValue<Vector2>();
        else
            return playerActions.FindAction("Steer").ReadValue<Vector2>();
    }
}
