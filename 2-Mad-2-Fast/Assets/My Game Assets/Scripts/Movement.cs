using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
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
    

    private InputAction steer;
    private InputAction leftPedal;
    private InputAction rightPedal;
    private InputAction brake;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        steer = playerControls.Player.Steer;
        steer.Enable();

        leftPedal = playerControls.Player.LeftPedal;
        leftPedal.Enable();
        leftPedal.performed += LeftPedal;

        rightPedal = playerControls.Player.RightPedal;
        rightPedal.Enable();
        rightPedal.performed += RightPedal;

        brake = playerControls.Player.Brake;
        brake.Enable();
        brake.performed += BrakePressed;
        brake.canceled += BrakeReleased;
    }
    private void OnDisable()
    {
        steer.Disable();
        leftPedal.Disable();
        rightPedal.Disable();
        brake.Disable();
    }

    private CharacterController characterContoller;
    Vector2 inputDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterContoller = GetComponent<CharacterController>();

        leftNext = true;
        rightNext = true;
        decelerationMultiplier = 1;
    }


    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        inputDirection = steer.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(inputDirection.x, 0, 1);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        speed -= decelerationPerTick * decelerationMultiplier;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();
        characterContoller.SimpleMove(movementDirection * speed);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void LeftPedal(InputAction.CallbackContext context)
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

    private void RightPedal(InputAction.CallbackContext context)
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

    private void BrakePressed(InputAction.CallbackContext context)
    {
        decelerationMultiplier = brakeDecelerationMultiplier;
    }
    private void BrakeReleased(InputAction.CallbackContext context)
    {
        decelerationMultiplier = 1;
    }
}
