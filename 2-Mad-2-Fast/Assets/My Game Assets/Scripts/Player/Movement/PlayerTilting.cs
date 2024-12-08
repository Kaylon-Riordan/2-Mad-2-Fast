using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

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

    PlayerPhysics pp;
    PlayerStumbler s;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
        s = GetComponent<PlayerStumbler>();
    }

    public void tiltBike(Vector2 inputDirection)
    {
        // Tilt the player in the direction of input, depending on speed
        pp.tilt += inputDirection.x * pp.speed * tiltRate;

        if (pp.tilt < 0)
        {
            pp.tilt += tiltRecovery;
            if (pp.tilt < -tiltLimit)
                StartCoroutine(s.Stumble(Penalty.Large, 0));
        }
        else if (pp.tilt > 0)
        {
            pp.tilt -= tiltRecovery;
            if (pp.tilt > tiltLimit)
                StartCoroutine(s.Stumble(Penalty.Large, 0));
        }

        pp.tiltShown = pivotPoint.localEulerAngles;
        pp.tiltShown.z = Mathf.Lerp(pp.tiltShown.z, -pp.tilt, tiltLerpSpeed * Time.deltaTime);
        pivotPoint.localEulerAngles = pp.tiltShown;
    }
}
