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
    public float fastSpeed;
    [SerializeField]
    public float mediumSpeed;

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

    [Header("Sound")]

    [SerializeField]
    private AudioClip windSound;

    // These traits change dynamically in other scripts
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public Speed speedBracket;
    [HideInInspector]
    public float minSpeed;

    [HideInInspector]
    public float decelerationMultiplier;


    private PlayerInputHandler inputHandler;
    private CharacterController characterContoller;

    // Steering
    private PlayerSteeringCalculator steering;

    private PlayerTilting tilting;
    private PlayerCollider playerCollider;

    public delegate void ChangeSpeed();
    public static ChangeSpeed changeSpeed;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        characterContoller = GetComponent<CharacterController>();

        steering = GetComponent<PlayerSteeringCalculator>();
        tilting = GetComponent<PlayerTilting>();
        playerCollider = GetComponent<PlayerCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        decelerationMultiplier = 1;
        speedBracket = Speed.Slow;
    }

    /// <summary>
    /// Main movement method that calls all the others and updates the players character contoller
    /// </summary>
    void FixedUpdate()
    {
        Vector3 movementDirection = steering.calculateDirection();
        tilting.tiltBike(steering.GetSteer());
        playerCollider.checkForContact();

        // Constantly decelerate, faster if brakes are applied
        speed -= decelerationPerTick * decelerationMultiplier;
        // Stop speed from exceding max or becoming negative
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        // update speed bracket
        if (speed >= fastSpeed && speedBracket != Speed.Fast)
        {
            //AudioManager.instance.PlaySound(windSound, AudioMixerGroupName.SFX, transform.position);
            // If the bike is fast but its enum isn't set to fast, update it
            speedBracket = Speed.Fast;
            // run the change speed delegate which will be ran inside the music script
            if (changeSpeed != null) {
                changeSpeed();
            }
        }
        else if (speed >= mediumSpeed && speed < fastSpeed && speedBracket != Speed.Medium)
        {
            // If player was slow and has moved up to medium, play wind sound
            if (speedBracket == Speed.Slow)
            {
                //AudioManager.instance.PlaySound(windSound, AudioMixerGroupName.SFX, transform.position);
            }
            speedBracket = Speed.Medium;
            if (changeSpeed != null)
            {
                changeSpeed();
            }
        }
        else if (speed < mediumSpeed && speedBracket != Speed.Slow)
        {
            speedBracket = Speed.Slow;
            if (changeSpeed != null)
            {
                changeSpeed();
            }
        }

        // Moves character using character contoller
        characterContoller.SimpleMove(movementDirection * speed);

        // Set pedal animation speed to be relative to player speed
        animator.speed = Mathf.Abs(speed / 5);

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
}