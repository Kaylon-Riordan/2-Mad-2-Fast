using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTilting : MonoBehaviour
{

    [HideInInspector]
    public float tilt;
    [HideInInspector]
    public Vector3 tiltShown;
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
        tilt += inputDirection.x * pp.speed * tiltRate;

        // Handle negative tilt
        if (tilt < 0)
        {
            //gradually return tilt to 0
            tilt += tiltRecovery;
            // If tilt passes the limit, cause the player to stumble and play sound
            if (tilt < -tiltLimit)
            {
                AudioManager.instance.PlaySound(stumbleSound, AudioMixerGroupName.SFX, transform.position);
                StartCoroutine(s.Stumble(Penalty.Large, 0, false));
            }
        }
        // Handle positive tilt
        else if (tilt > 0)
        {
            tilt -= tiltRecovery;
            if (tilt > tiltLimit)
            {
                AudioManager.instance.PlaySound(stumbleSound, AudioMixerGroupName.SFX, transform.position);
                StartCoroutine(s.Stumble(Penalty.Large, 0, false));
            }
        }

        // Get current roatation of model
        tiltShown = pivotPoint.localEulerAngles;
        // Gradually shift the models rotation towards the tilt variable
        tiltShown.z = Mathf.Lerp(tiltShown.z, -tilt, tiltLerpSpeed * Time.deltaTime);

        pivotPoint.localEulerAngles = tiltShown;
    }
}
