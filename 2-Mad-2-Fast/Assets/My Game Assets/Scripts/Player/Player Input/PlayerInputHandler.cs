using UnityEngine;

/// <summary>
/// Receives inputs from Player and sends events that trigger each action
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputBinder b;
    private PlayerController controller;

    private void Awake()
    {
        b = GetComponent<PlayerInputBinder>();
        controller = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        // Execute methods if performed
        b.leftPedal.performed += controller.LeftPedal;
        b.rightPedal.performed += controller.RightPedal;
        b.brake.performed += controller.BrakePressed;
        b.brake.canceled += controller.BrakeReleased;

        b.playerActions.Enable();
    }

    private void OnDisable()
    {
        b.playerActions.Disable();
    }
}
