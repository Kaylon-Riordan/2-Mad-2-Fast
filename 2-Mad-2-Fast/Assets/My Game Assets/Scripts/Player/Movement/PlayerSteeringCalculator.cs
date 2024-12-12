using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteeringCalculator : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    private PlayerInputBinder b;

    private void Awake()
    {
        b = GetComponent<PlayerInputBinder>();
    }

    public Vector2 GetSteer()
    {
        if (PlayerManager.instance.ip.controlSchemes[b.PlayerNo - 1] == ControlScheme.Shared)
            return b.playerActions.FindAction("SteerP" + b.PlayerNo).ReadValue<Vector2>();
        else
            return b.playerActions.FindAction("Steer").ReadValue<Vector2>();
    }

    public Vector3 calculateDirection()
    {
        Vector3 movementDirection = new Vector3(GetSteer().x, 0, 1);
        // Store magnitude without exceding 1 on diagnal inputs
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        // Set movement to direction of camera
        movementDirection = Quaternion.AngleAxis(cameraTransform.
            rotation.eulerAngles.y, Vector3.up) * movementDirection;
        // Remove magnitude, leaving only direction (i.e create a unit vector)
        movementDirection.Normalize();
        // Multiply by magnitude for horizontal analogue inputs
        movementDirection.x *= inputMagnitude;

        return movementDirection;
    }
}
