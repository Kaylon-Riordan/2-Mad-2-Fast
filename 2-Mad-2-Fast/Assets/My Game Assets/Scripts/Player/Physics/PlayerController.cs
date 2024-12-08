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

    PlayerPhysics pp;
    PlayerRhythmTracker rhythm;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
        rhythm = GetComponent<PlayerRhythmTracker>();
    }

    private void Start()
    {
        leftNext = true;
        rightNext = true;
    }

    public void LeftPedal(InputAction.CallbackContext context)
    {
        if (leftNext)
        {
            leftNext = false;
            rightNext = true;

            pp.speed += accelerationPerPedal * rhythm.CheckRhythm();
        }
        else
        {
            pp.speed -= decelerationPerMiss;
        }
    }

    public void RightPedal(InputAction.CallbackContext context)
    {
        if (rightNext)
        {
            rightNext = false;
            leftNext = true;

            pp.speed += accelerationPerPedal * rhythm.CheckRhythm();
        }
        else
        {
            pp.speed -= decelerationPerMiss;
        }
    }

    public void BrakePressed(InputAction.CallbackContext context)
    {
        pp.decelerationMultiplier = brakeMultiplier;
    }
    public void BrakeReleased(InputAction.CallbackContext context)
    {
        pp.decelerationMultiplier = 1;
    }
}
