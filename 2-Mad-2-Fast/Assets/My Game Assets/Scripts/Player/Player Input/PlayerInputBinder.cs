using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Binds a player's inputs as according to their desired control scheme
/// </summary>
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
            getSharedInputDeviceBindings();
        }
        else
        {
            playerActions = playerActionAsset.FindActionMap("Player");
            getIndependentInputDeviceBindings();
        }
    }

    /// <summary>
    /// returns default/onehanded device bindings
    /// </summary>
    private void getIndependentInputDeviceBindings()
    {
        // Find Actions
        steer = playerActions.FindAction("Steer");
        leftPedal = playerActions.FindAction("LeftPedal");
        rightPedal = playerActions.FindAction("RightPedal");
        brake = playerActions.FindAction("Brake");
    }

    /// <summary>
    /// Returns shared device bindings
    /// </summary>
    // Called where each player shares a single input device
    private void getSharedInputDeviceBindings()
    {
        // Find Actions
        steer = playerActions.FindAction("SteerP" + PlayerNo);
        leftPedal = playerActions.FindAction("LeftPedalP" + PlayerNo);
        rightPedal = playerActions.FindAction("RightPedalP" + PlayerNo);
        brake = playerActions.FindAction("BrakeP" + PlayerNo);
    }
}
