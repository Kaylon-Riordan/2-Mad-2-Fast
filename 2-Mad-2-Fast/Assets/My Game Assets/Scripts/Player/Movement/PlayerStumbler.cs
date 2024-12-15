using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStumbler : MonoBehaviour
{

    [Header("Penalties")]

    [SerializeField]
    private float largePenalty;
    [SerializeField]
    private float mediumPenalty;
    [SerializeField]
    private float smallPenalty;

    [Header("Sound")]
    [SerializeField]
    private AudioClip crashSound;

    private bool slowed;

    PlayerPhysics pp;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
    }

    private void Start()
    {
        slowed = false;
    }

    private float PenaltyToFloat(Penalty penalty)
    {
        switch (penalty)
        {
            case Penalty.Small:
                return smallPenalty;
            case Penalty.Medium:
                return mediumPenalty;
            case Penalty.Large:
                return largePenalty;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Penalize the players speed when they make a mistake e.g. hit wall or turn too fast
    /// </summary>
    /// <param name="penalty"> Percentage or origional speed that new speed will be set to </param>
    /// <param name="delay"> How long to wait before performing stumble </param>
    /// <param name="collision"> Was the stumble caused by a collision </param>
    /// <returns></returns>
    public IEnumerator Stumble(Penalty penalty, float delay, bool collision)
    {
        // convert enum to flaot
        float multiplier = PenaltyToFloat(penalty);

        // Wait a short period before triggering code, this delay is to check if a side
            // swipe will become a front on collision and one collider was just slightly first
        yield return new WaitForSeconds(delay);
        // Don't run the code if the player has already been slowed recently
        if (!slowed)
        {
            // Play crash sound if it was a collision and at least a medium impact
            if (collision && multiplier <= mediumPenalty)
            {
                AudioManager.instance.PlaySound(crashSound, AudioMixerGroupName.SFX, transform.position);
            }
            // Set slow to true and, decrese speed
            slowed = true;
            pp.speed *= multiplier;

            // Briefly wait before turning slowed back to false
            yield return new WaitForSeconds(2f);
            slowed = false;
        }
    }
}
