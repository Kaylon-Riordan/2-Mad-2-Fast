using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerControls playerControls;

    private InputAction steer;
    private InputAction leftPedal;
    private InputAction rightPedal;
    private InputAction brake;

    private playerController controller;

    private void Awake()
    {
        playerControls = new PlayerControls();
        controller = GetComponent<playerController>();
    }

    private void OnEnable()
    {
        steer = playerControls.Player.Steer;
        steer.Enable();

        leftPedal = playerControls.Player.LeftPedal;
        leftPedal.Enable();
        leftPedal.performed += controller.LeftPedal;

        rightPedal = playerControls.Player.RightPedal;
        rightPedal.Enable();
        rightPedal.performed += controller.RightPedal;

        brake = playerControls.Player.Brake;
        brake.Enable();
        brake.performed += controller.BrakePressed;
        brake.canceled += controller.BrakeReleased;
    }

    private void OnDisable()
    {
        steer.Disable();
        leftPedal.Disable();
        rightPedal.Disable();
        brake.Disable();
    }

    public Vector2 GetSteer()
    {
        return steer.ReadValue<Vector2>();
    }
}
