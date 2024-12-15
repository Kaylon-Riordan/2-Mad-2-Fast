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

    /// <summary>
    /// Checks if there is similar timing between each pedal, give bonus speed if there is
    /// </summary>
    /// <returns> 1 for bad rhythm or a positive speed multiplier for good rhythm </returns>
    public float CheckRhythm()
    {
        // Keep track of the last 6 times pedals were pressed
        pedalTiming[5] = pedalTiming[4];
        pedalTiming[4] = pedalTiming[3];
        pedalTiming[3] = pedalTiming[2];
        pedalTiming[2] = pedalTiming[1];
        pedalTiming[1] = pedalTiming[0];
        pedalTiming[0] = Time.time;

        // After pedals have been pressed 6 times start this code
        if (pedalTiming[5] != 0f)
        {
            // Get average time between the last 5 pedals before the most recent
            average = ((pedalTiming[1] - pedalTiming[2]) + (pedalTiming[2] - pedalTiming[3]) + (pedalTiming[3] - pedalTiming[4]) + (pedalTiming[4] - pedalTiming[5])) / 4;

            // Give rhythm bonus if the time between the 2 most recent pedals was within the tolerance rating of the average
            if ((pedalTiming[0] - pedalTiming[1]) < (average + average * rhythmTolerance) && (pedalTiming[0] - pedalTiming[1]) > (average - average * rhythmTolerance))
            {
                return rhythmMultiplier;
            }
        }
        return 1;
    }
}