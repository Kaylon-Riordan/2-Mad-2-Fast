using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTilting : MonoBehaviour
{


    [SerializeField]
    private float tiltLimit;
    [SerializeField]
    private float tiltRecovery;
    [SerializeField]
    private float tiltRate;
    [SerializeField]
    private float tiltLerpSpeed;
    [SerializeField]
    public Transform pivotPoint;

    [Header("Sound")]
    [SerializeField]
    private AudioClip stumbleSound;

    PlayerPhysics pp;
    PlayerStumbler s;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
        s = GetComponent<PlayerStumbler>();
    }

    /// <summary>
    /// Cause the player to tilt  while turning, angle determined by how ahrd they turn and their speed
    /// </summary>
    /// <param name="inputDirection"> Directional input given by player </param>
    public void tiltBike(Vector2 inputDirection)
    {
        // Tilt the player in the direction of input, depending on speed
        pp.tilt += inputDirection.x * pp.speed * tiltRate;

        // Handle negative tilt
        if (pp.tilt < 0)
        {
            //gradually return tilt to 0
            pp.tilt += tiltRecovery;
            // If tilt passes the limit, cause the player to stumble and play sound
            if (pp.tilt < -tiltLimit)
            {
                AudioManager.instance.PlaySound(stumbleSound, AudioMixerGroupName.SFX, transform.position);
                StartCoroutine(s.Stumble(Penalty.Large, 0, false));
            }
        }
        // Handle positive tilt
        else if (pp.tilt > 0)
        {
            pp.tilt -= tiltRecovery;
            if (pp.tilt > tiltLimit)
            {
                AudioManager.instance.PlaySound(stumbleSound, AudioMixerGroupName.SFX, transform.position);
                StartCoroutine(s.Stumble(Penalty.Large, 0, false));
            }
        }

        // Get current roatation of model
        pp.tiltShown = pivotPoint.localEulerAngles;
        // Gradually shift the models rotation towards the tilt variable
        pp.tiltShown.z = Mathf.Lerp(pp.tiltShown.z, -pp.tilt, tiltLerpSpeed * Time.deltaTime);
        pivotPoint.localEulerAngles = pp.tiltShown;
    }
}
