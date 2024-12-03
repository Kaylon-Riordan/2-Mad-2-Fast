using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    // Physics Attributes
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxReverseSpeed;
    [SerializeField]
    private float fastSpeed;
    [SerializeField]
    private float moderateSpeed;
    [SerializeField]
    private float largePenalty;
    [SerializeField]
    private float mediumPenalty;
    [SerializeField]
    private float smallPenalty;
    [SerializeField]
    private float accelerationPerPedal;
    [SerializeField]
    private float decelerationPerMiss;
    [SerializeField]
    private float decelerationPerTick;
    [SerializeField]
    private float brakeMultiplier;
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
    private float rhythmMultiplier;
    [SerializeField]
    private float rhythmTolerance;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform pivotPoint;
    [SerializeField]
    private Bumper leftCollisionDetector;
    [SerializeField]
    private Bumper rightCollisionDetector;

    private float speed;
    private float minSpeed;
    private float tilt;
    private bool slowed;
    private Vector3 tiltShown;
    private bool leftNext;
    private bool rightNext;
    private float decelerationMultiplier;
    private float[] pedalTiming = new float[6];
    private float average;

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
        speed = 0;
        minSpeed = 0;
        slowed = false;
        leftNext = true;
        rightNext = true;
        decelerationMultiplier = 1;
    }

    // Basic movement code in fixed update and calculate direction based off first 5 videos of this playlist https://www.youtube.com/playlist?list=PLx7AKmQhxJFaj0IcdjGJzIq5KwrIfB1m9

    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        Vector3 movementDirection = calculateDirection(inputHandler.GetSteer());
        tiltBike(inputHandler.GetSteer());
        handleCollision();

        // Moves character using character contoller
        characterContoller.SimpleMove(movementDirection * speed);

        if (movementDirection != Vector3.zero)
        {
            // Get direction character should face from movement
            Quaternion toRotation = Quaternion.LookRotation
                (movementDirection, Vector3.up);
            // Rotate towards that direction at rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.
                rotation, toRotation, turningSpeed * Time.deltaTime);
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
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        // Set movement to direction of camera
        movementDirection = Quaternion.AngleAxis(cameraTransform.
            rotation.eulerAngles.y, Vector3.up) * movementDirection;
        // Remove magnitude, leaving only direction (i.e create a unit vector)
        movementDirection.Normalize();
        // Multiply by magnitude for horizontal analogue inputs
        movementDirection.x *= inputMagnitude;

        return movementDirection;
    }

    private void tiltBike(Vector2 inputDirection)
    {
        // Tilt the player in the direction of input, depending on speed
        tilt += inputDirection.x * speed * tiltRate;

        // Handle negative tilt
        if (tilt < 0)
        {
            //gradually return tilt to 0
            tilt += tiltRecovery;
            // If tilt passes the limit, cause the player to stumble
            if (tilt < -tiltLimit)
            {
                StartCoroutine(Stumble(largePenalty, 0));
            }
        }
        // Handle positive tilt
        else if (tilt > 0)
        {
            tilt -= tiltRecovery;
            if (tilt > tiltLimit)
            {
                StartCoroutine(Stumble(largePenalty, 0));
            }
        }

        // Get current roatation of model
        tiltShown = pivotPoint.localEulerAngles;
        // Gradually shift the models rotation towards the tilt variable
        tiltShown.z = Mathf.Lerp(tiltShown.z, -tilt, tiltLerpSpeed * Time.deltaTime);
        pivotPoint.localEulerAngles = tiltShown;
    }

    // Uses 2 colliders on the front left and front right of the boke to determin how it contacted other objects
    private void handleCollision()
    {
        // If both colliders make contact, a front on collision, apply a harsh penalty dependant on speed
        if (leftCollisionDetector.contact && rightCollisionDetector.contact)
        {
            if (speed >= fastSpeed)
            {
                StartCoroutine(Stumble(largePenalty, 0));
            }
            else if (speed >= moderateSpeed)
            {
                StartCoroutine(Stumble(mediumPenalty, 0));
            }
            else
            {
                StartCoroutine(Stumble(smallPenalty, 0));
            }
        }
        // If one collider makes contact, a side swipe, apply a light penalty dependant on speed
        else if (leftCollisionDetector.contact || rightCollisionDetector.contact)
        {
            if (speed >= fastSpeed)
            {
                StartCoroutine(Stumble(mediumPenalty, 0.05f));
            }
            else if (speed >= moderateSpeed)
            {
                StartCoroutine(Stumble(smallPenalty, 0.05f));
            }
        }
    }

    IEnumerator Stumble(float penalty, float delay)
    {
        // Wait a short period before triggering code, this delay is to check if a side
        // swipe will become a front on collision and one collider was just slightly first.
        yield return new WaitForSeconds(delay); 
        // Don't run the code if the player has already been slowed recently
        if (!slowed)
        {
            // Set slow to true and, decrese speed
            slowed = true;
            speed *= penalty;

            // Briefly wait before turning slowed back to false
            yield return new WaitForSeconds(2f);
            slowed = false;
        }
    }

    private float CheckRhythm()
    {
        // Keep track of the last 6 times pedals were pressed
        pedalTiming[5] = pedalTiming[4];
        pedalTiming[4] = pedalTiming[3];
        pedalTiming[3] = pedalTiming[2];
        pedalTiming[2] = pedalTiming[1];
        pedalTiming[1] = pedalTiming[0];
        pedalTiming[0] = Time.time;

        // After pedals have been pressed 6 times start this code
        if (pedalTiming[5] != 0f)
        {
            // Get average time between the last 5 pedals before the most recent
            average = ((pedalTiming[1] - pedalTiming[2]) + (pedalTiming[2] - pedalTiming[3]) + (pedalTiming[3] - pedalTiming[4]) + (pedalTiming[4] - pedalTiming[5])) / 4;

            // Give rhythm bonus if the time between the 2 most recent pedals was within the tolerance rating of the average
            if ((pedalTiming[0] - pedalTiming[1]) < (average + average * rhythmTolerance) && (pedalTiming[0] - pedalTiming[1]) > (average - average * rhythmTolerance))
            {
                return rhythmMultiplier;
            }
        }

        // Otherwise give no speed bonus
        return 1;
    }

    public void LeftPedal(InputAction.CallbackContext context)
    {
        // Increase speed if the right pedal was pressed
        if(leftNext)
        {
            // Swap next expected petal
            leftNext = false;
            rightNext = true;

            // Increase speed and apply rhythym multiplier if earned
            speed += accelerationPerPedal * CheckRhythm();
        }
        // Decrease speed if the wrong pedal was pressed
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

            speed += accelerationPerPedal * CheckRhythm();
        }
        else
        {
            speed -= decelerationPerMiss;
        }
    }

    // When brake is pressed decelaerate quickly
    public void BrakePressed(InputAction.CallbackContext context)
    {
        decelerationMultiplier = brakeMultiplier;
        minSpeed = -maxReverseSpeed;
    }
    // When brake is released decelaerate normally
    public void BrakeReleased(InputAction.CallbackContext context)
    {
        decelerationMultiplier = 1;
        minSpeed = 0;
    }
}
