using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Learned how to use delegates from https://youtu.be/J01z1F-du-E
public class PlayerCountdown : MonoBehaviour
{
    [SerializeField]
    private AudioClip countdownSound;
    // create a public delegate other classes can access
    public delegate void AfterCountdown();
    public static AfterCountdown afterCountdown;

    /// <summary>
    /// Metod to start coroutine
    /// </summary>
    public void StartCountDown()
    {
        StartCoroutine(Countdown());
    }
    /// <summary>
    /// Play a count down timer and let players start when its finished
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        // Play count down sound
        AudioManager.instance.PlaySound(countdownSound, AudioMixerGroupName.SFX, transform.position);
        // Wait for slightly shorter time than the sound
        yield return new WaitForSeconds(countdownSound.length - 0.5f);
        // Trigger the delegate to let players start
        if (afterCountdown != null)
        {
            afterCountdown();
        }
    }
}
