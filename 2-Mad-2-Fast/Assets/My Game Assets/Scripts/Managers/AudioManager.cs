using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Game.Pool;
using Unity.VisualScripting;

// Code based on Niall McGuinness's https://github.com/nmcguinness/2024-25-GD3A-IntroToUnity

public class AudioManager : MonoBehaviour
{
    // Creates an empty instance of this script
    public static AudioManager instance;

    [Header("Prefab")]
    [SerializeField]
    [Tooltip("The prefab used to instantiate AudioSources.")]
    private AudioSource audioSourcePrefab;

    [SerializeField]
    [Range(1, 32)]
    [Tooltip("The initial size of the AudioSource pool.")]
    private int initialPoolSize = 8;

    [Header("Mixer")]
    [SerializeField]
    [Tooltip("The AudioMixer used to control the audio groups.")]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioMixerGroup masterGroup;
    [SerializeField]
    private AudioMixerGroup sfxGroup;
    [SerializeField]
    private AudioMixerGroup musicGroup;

    private GameObjectPool<AudioSource> audioSourcePool;

    void Awake()
    {
        // Initialize the AudioSource pool
        audioSourcePool = new GameObjectPool<AudioSource>(audioSourcePrefab, initialPoolSize, transform);

        instance = Singleton<AudioManager>.get();
    }

    /// <summary>
    /// Gets the AudioMixerGroup associated with the given AudioMixerGroupName.
    /// </summary>
    /// <param name="groupName">The AudioMixerGroupName enum value.</param>
    /// <returns>The corresponding AudioMixerGroup.</returns>
    public AudioMixerGroup GetAudioMixerGroup(AudioMixerGroupName groupName)
    {
        // Expression based switch from C# 8.0
        // https://www.c-sharpcorner.com/article/c-sharp-8-0-new-feature-swtich-expression/
        return groupName switch
        {
            AudioMixerGroupName.Master => masterGroup,
            AudioMixerGroupName.SFX => sfxGroup,
            AudioMixerGroupName.Music => musicGroup,
            _ => null,
        };
    }

    public void PlaySound(AudioClip clip, AudioMixerGroupName groupName, Vector3 position = default)
    {
        AudioSource audioSource = audioSourcePool.Get();
        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(groupName);
        audioSource.Play();

        StartCoroutine(ReturnAudioSourceAfterPlaying(audioSource));

    }

    public void PlayMusic(AudioClip clip, AudioMixerGroupName groupName, ref bool loop, Vector3 position = default)
    {
        AudioSource audioSource = audioSourcePool.Get();
        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(groupName);
        audioSource.Play();

        while (loop) {}
        StartCoroutine(ReturnAudioSourceAfterPlaying(audioSource));
    }

    private IEnumerator ReturnAudioSourceAfterPlaying(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSourcePool.ReturnToPool(audioSource);
    }
}
