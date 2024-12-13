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

    public void StartCountDown()
    {
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        AudioManager.instance.PlaySound(countdownSound, AudioMixerGroupName.SFX, transform.position);
        yield return new WaitForSeconds(countdownSound.length - 0.5f);
        // trigger the delegate
        if (afterCountdown != null)
        {
            afterCountdown();
        }
    }
}
