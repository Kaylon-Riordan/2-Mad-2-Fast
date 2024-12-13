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


    public void LeftPedal(InputAction.CallbackContext context)
    {
        if (go)
        {
            if (leftNext)
            {
                AudioManager.instance.PlaySound(pedalSound, AudioMixerGroupName.SFX, transform.position);
                leftNext = false;
                rightNext = true;

                pp.speed += accelerationPerPedal * rhythm.CheckRhythm();
            }
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

    // When brake is pressed decelaerate quickly
    public void BrakePressed(InputAction.CallbackContext context)
    {
        if (go)
        {
            AudioManager.instance.PlaySound(brakeSound, AudioMixerGroupName.SFX, transform.position);
            pp.decelerationMultiplier = brakeMultiplier;
            pp.minSpeed = -maxReverseSpeed;
        }
    }
    // When brake is released decelaerate normally
    public void BrakeReleased(InputAction.CallbackContext context)
    {
        pp.decelerationMultiplier = 1;
        pp.minSpeed = 0;
    }
}
