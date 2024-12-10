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

    public IEnumerator Stumble(Penalty penalty, float delay, bool collision)
    {
        float multiplier = PenaltyToFloat(penalty);

        yield return new WaitForSeconds(delay);
        if (!slowed)
        {
            if (collision && multiplier <= mediumPenalty)
            {
                AudioManager.instance.PlaySound(crashSound, AudioMixerGroupName.SFX, false, transform.position);
            }
            slowed = true;
            pp.speed *= multiplier;

            yield return new WaitForSeconds(2f);

            slowed = false;
        }
    }
}
