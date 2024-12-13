using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Speed")]

    // Physics Attributes
    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float turningSpeed;

    [Header("Acceleration")]

    [SerializeField]
    private float decelerationPerTick;

    [Header("Controls")]

    [SerializeField]
    private PlayerControls playerControls;

    [Header("Animation")]

    [SerializeField]
    private Animator animator;

    // These traits change dynamically in other scripts
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float minSpeed;
    [HideInInspector]
    public float tilt;
    [HideInInspector]
    public Vector3 tiltShown;
    [HideInInspector]
    public float decelerationMultiplier;

    private bool go = false;
    private bool first = true;

    private PlayerInputHandler inputHandler;
    private CharacterController characterContoller;

    // Steering
    private PlayerSteeringCalculator steering;

    private PlayerTilting tilting;
    private PlayerCollider collider;
    private PlayerCountdown countdown;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        characterContoller = GetComponent<CharacterController>();

        steering = GetComponent<PlayerSteeringCalculator>();
        tilting = GetComponent<PlayerTilting>();
        collider = GetComponent<PlayerCollider>();
        countdown = GetComponent<PlayerCountdown>();
    }

    // Start is called before the first frame update
    void Start()
    {
        decelerationMultiplier = 1;
        countdown.StartCountDown();
        countdown.OnCountdownComplete = () =>
        {
            go = true;
        };
    }

    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        if (go || first) {
            Vector3 movementDirection = steering.calculateDirection();
            tilting.tiltBike(steering.GetSteer());
            collider.checkForContact();

            // Constantly decelerate, faster if brakes are applied
            speed -= decelerationPerTick * decelerationMultiplier;
            // Stop speed from exceding max or becoming negative
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            // Moves character using character contoller
            characterContoller.SimpleMove(movementDirection * speed);

            // Set pedal animation speed to be relative to player speed
            animator.speed = speed / 5;

            if (movementDirection != Vector3.zero)
            {
                // Get direction character should face from movement
                Quaternion toRotation = Quaternion.LookRotation
                    (movementDirection, Vector3.up);
                // Rotate towards that direction at rotation speed
                transform.rotation = Quaternion.RotateTowards(transform.
                    rotation, toRotation, turningSpeed * Time.deltaTime);
            }
            first = false;
        }
    }
}