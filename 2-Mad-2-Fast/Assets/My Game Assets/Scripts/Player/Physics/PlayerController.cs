using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool leftNext;
    private bool rightNext;

    // Acceleration
    [SerializeField]
    private float accelerationPerPedal;
    [SerializeField]
    private float decelerationPerMiss;

    // Brake
    [SerializeField]
    private float brakeMultiplier;
    [SerializeField]
    private float maxReverseSpeed;

    [SerializeField]
    private AudioClip brakeSound;
    [SerializeField]
    private AudioClip pedalSound;

    PlayerPhysics pp;
    PlayerRhythmTracker rhythm;
    private PlayerCountdown countdown;

    private bool go = false;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
        rhythm = GetComponent<PlayerRhythmTracker>();
        countdown = GetComponent<PlayerCountdown>();
    }

    private void Start()
    {
        leftNext = true;
        rightNext = true;

        // start the countdown in its own method then add the go method to the delegate so when its triggered go becomes true
        countdown.StartCountDown();
        PlayerCountdown.afterCountdown += Go;
    }
    void Go()
    {
        go = true;
        PlayerCountdown.afterCountdown -= Go;
    }

    /// <summary>
    /// increase players speed if they hit the correct pedal and decrease if they hit the wrong one
    /// </summary>
    /// <param name="context"> Signals the left pedal input was given </param>
    public void LeftPedal(InputAction.CallbackContext context)
    {
        // Only allow pedaling if the starting countdown has finished
        if (go)
        {
            // Increase speed if the right pedal was pressed
            if (leftNext)
            {
                // Play pedal sound
                AudioManager.instance.PlaySound(pedalSound, AudioMixerGroupName.SFX, transform.position);
                // Swap next expected petal
                leftNext = false;
                rightNext = true;
                // Increase speed and apply rhythym multiplier if earned
                pp.speed += accelerationPerPedal * rhythm.CheckRhythm();
            }
            // Decrease speed if the wrong pedal was pressed
            else
            {
                pp.speed -= decelerationPerMiss;
            }
        }
    }
    public void RightPedal(InputAction.CallbackContext context)
    {
        if (go)
        {
            if (rightNext)
            {
                AudioManager.instance.PlaySound(pedalSound, AudioMixerGroupName.SFX, transform.position);
                rightNext = false;
                leftNext = true;

                pp.speed += accelerationPerPedal * rhythm.CheckRhythm();
            }
            else
            {
                pp.speed -= decelerationPerMiss;
            }
        }
    }

    /// <summary>
    /// When brake is pressed decelaerate quickly or reverse if at a stop
    /// </summary>
    /// <param name="context"> Signals the brake input was given </param>
    public void BrakePressed(InputAction.CallbackContext context)
    {
        // Only allow braking if the starting countdown has finished
        if (go)
        {
            // Play brake sound
            AudioManager.instance.PlaySound(brakeSound, AudioMixerGroupName.SFX, transform.position);
            // Decelerate Faster
            pp.decelerationMultiplier = brakeMultiplier;
            // Allow negative speed for reversing
            pp.minSpeed = -maxReverseSpeed;
        }
    }
    /// <summary>
    /// When brake is released decelaerate normally
    /// </summary>
    /// <param name="context"> Signals the brake input was given </param>
    public void BrakeReleased(InputAction.CallbackContext context)
    {
        // Decelerate normally
        pp.decelerationMultiplier = 1;
        // Stop reversing
        pp.minSpeed = 0;
    }
}
