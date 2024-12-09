using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRhythmTracker : MonoBehaviour
{
    [SerializeField]
    private float rhythmMultiplier;
    [SerializeField]
    private float rhythmTolerance;

    private float average;
    private float[] pedalTiming = new float[6];

    public float CheckRhythm()
    {
        pedalTiming[5] = pedalTiming[4];
        pedalTiming[4] = pedalTiming[3];
        pedalTiming[3] = pedalTiming[2];
        pedalTiming[2] = pedalTiming[1];
        pedalTiming[1] = pedalTiming[0];
        pedalTiming[0] = Time.time;

        if (pedalTiming[5] != 0f)
        {
            average = ((pedalTiming[1] - pedalTiming[2]) + (pedalTiming[2] - pedalTiming[3]) + (pedalTiming[3] - pedalTiming[4]) + (pedalTiming[4] - pedalTiming[5])) / 4;
            if ((pedalTiming[0] - pedalTiming[1]) < (average + average * rhythmTolerance) && (pedalTiming[0] - pedalTiming[1]) > (average - average * rhythmTolerance))
            {
                return rhythmMultiplier;
            }
        }

        return 1;
    }
}