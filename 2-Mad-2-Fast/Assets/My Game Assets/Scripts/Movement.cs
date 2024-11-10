using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private float turningSpeed;
    [SerializeField]
    private float tiltLimit;
    [SerializeField]
    private float tiltRecovery;
    [SerializeField]
    private float tiltRate;
    [SerializeField]
    private float tiltLerpSpeed;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform pivotPoint;
    [SerializeField]
    private PlayerControls playerControls;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float tilt;
    [SerializeField]
    private bool stumble;
    [SerializeField]
    private Vector3 tiltShown;
    

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

        stumble = false;
        leftNext = true;
        rightNext = true;
        decelerationMultiplier = 1;
    }


    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        if(!stumble)
        {
            // Take input direction from user and store in vector 3
            inputDirection = steer.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(inputDirection.x, 0, 1);
            // Store magnitude without exceding 1 on diagnal inputs
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

            // Constantly decelerate, faster if brakes are applied
            speed -= decelerationPerTick * decelerationMultiplier;
            // Stop speed from exceding max or becoming negative
            speed = Mathf.Clamp(speed, 0, maxSpeed);
            // Tilt the player in the direction of input, depending on speed
            tilt += inputDirection.x * speed * tiltRate;

            // Set movement to direction of camera
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
            // Remove magnitude, leaving only direction
            movementDirection.Normalize();
            // Multiply by magnitude for horizontal analogue inputs
            movementDirection.x *= inputMagnitude;
            // Moves character using character contoller
            characterContoller.SimpleMove(movementDirection * speed);

            if (movementDirection != Vector3.zero)
            {
                // Get direction character should face from movement
                Quaternion toRotation = Quaternion.LookRotation
                    (movementDirection, Vector3.up);
                // Rotate towards that direction at rotation speed
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turningSpeed * Time.deltaTime);
            }
        }

        if (tilt < 0)
        {
            tilt += tiltRecovery;
            if (tilt < -tiltLimit)
            {
                StartCoroutine(Stumble());
            }
        }
        else if (tilt > 0)
        {
            tilt -= tiltRecovery;
            if (tilt > tiltLimit)
            {
                StartCoroutine(Stumble());
            }
        }

        tiltShown = pivotPoint.localEulerAngles;
        tiltShown.z = Mathf.Lerp(tiltShown.z, -tilt, tiltLerpSpeed * Time.deltaTime);
        pivotPoint.localEulerAngles = tiltShown;
    }

    IEnumerator Stumble()
    {
        stumble = true;
        speed = 0;

        yield return new WaitForSeconds(2f);

        stumble = false;
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
