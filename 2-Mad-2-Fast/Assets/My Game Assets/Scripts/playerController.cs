using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class playerController : MonoBehaviour
{
    // Physics Attributes
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationPerPedal;
    [SerializeField]
    private float decelerationPerMiss;
    [SerializeField]
    private float decelerationPerTick;
    [SerializeField]
    private float brakeDecelerationMultiplier;
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private PlayerControls playerControls;

    [SerializeField]
    private float speed;
    private bool leftNext;
    private bool rightNext;
    private float decelerationMultiplier;

    private playerInputHandler inputHandler;
    private CharacterController characterContoller;

    private void Awake()
    {
        inputHandler = GetComponent<playerInputHandler>();
        characterContoller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        leftNext = true;
        rightNext = true;
        decelerationMultiplier = 1;
    }

    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        Vector3 movementDirection = calculateDirection(inputHandler.GetSteer());

        // Moves character using character contoller
        characterContoller.SimpleMove(movementDirection * speed);

        if (movementDirection != Vector3.zero)
        {
            // Get direction character should face from movement
            Quaternion toRotation = Quaternion.LookRotation
                (movementDirection, Vector3.up);
            // Rotate towards that direction at rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.
                rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 calculateDirection(Vector2 inputDirection)
    {

        Vector3 movementDirection = new Vector3(inputDirection.x, 0, 1);
        // Store magnitude without exceding 1 on diagnal inputs
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        // Constantly decelerate, faster if brakes are applied
        speed -= decelerationPerTick * decelerationMultiplier;
        // Stop speed from exceding max or becoming negative
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        // Set movement to direction of camera
        movementDirection = Quaternion.AngleAxis(cameraTransform.
            rotation.eulerAngles.y, Vector3.up) * movementDirection;
        // Remove magnitude, leaving only direction (i.e create a unit vector)
        movementDirection.Normalize();
        // Multiply by magnitude for horizontal analogue inputs
        movementDirection.x *= inputMagnitude;

        return movementDirection;
    }

    public void LeftPedal(InputAction.CallbackContext context)
    {
        if(leftNext)
        {
            leftNext = false;
            rightNext = true;

            speed += accelerationPerPedal;
        }
        else
        {
            speed -= decelerationPerMiss;
        }
    }

    public void RightPedal(InputAction.CallbackContext context)
    {
        if (rightNext)
        {
            rightNext = false;
            leftNext = true;

            speed += accelerationPerPedal;
        }
        else
        {
            speed -= decelerationPerMiss;
        }
    }

    public void BrakePressed(InputAction.CallbackContext context)
    {
        decelerationMultiplier = brakeDecelerationMultiplier;
    }
    public void BrakeReleased(InputAction.CallbackContext context)
    {
        decelerationMultiplier = 1;
    }
}
