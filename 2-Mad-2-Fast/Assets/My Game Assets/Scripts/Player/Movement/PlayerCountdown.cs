using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCountdown : MonoBehaviour
{
    [SerializeField]
    private AudioClip countdownSound;
    public System.Action OnCountdownComplete;

    public void StartCountDown()
    {
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        AudioManager.instance.PlaySound(countdownSound, AudioMixerGroupName.SFX, transform.position);
        yield return new WaitForSeconds(countdownSound.length);
        OnCountdownComplete?.Invoke();
    }
}
