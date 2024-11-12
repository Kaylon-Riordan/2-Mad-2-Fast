using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputHandler : MonoBehaviour
{
    private InputActionAsset playerActionAsset;
    private InputActionMap playerControls;

    private InputAction steer;
    private InputAction leftPedal;
    private InputAction rightPedal;
    private InputAction brake;

    private playerController controller;

    private void Awake()
    {
        playerActionAsset = GetComponent<PlayerInput>().actions;
        playerControls = playerActionAsset.FindActionMap("Player");
        controller = GetComponent<playerController>();
    }

    private void OnEnable()
    {
        // Find Actions
        steer = playerControls.FindAction("Steer");
        leftPedal = playerControls.FindAction("LeftPedal");
        rightPedal = playerControls.FindAction("RightPedal");
        brake = playerControls.FindAction("Brake");

        // Execute methods if performed
        leftPedal.performed += controller.LeftPedal;
        rightPedal.performed += controller.RightPedal;
        brake.performed += controller.BrakePressed;
        brake.canceled += controller.BrakeReleased;

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetSteer()
    {
        return playerControls.FindAction("Steer").ReadValue<Vector2>();
    }
}
