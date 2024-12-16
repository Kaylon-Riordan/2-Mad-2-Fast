using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Game.Pool;

// Code based on Niall McGuinness's https://github.com/nmcguinness/2024-25-GD3A-IntroToUnity

public class AudioManager : MonoBehaviour
{
    // Creates an empty instance of this script
    public static AudioManager instance;

    [Header("Prefab")]
    [SerializeField]
    [Tooltip("The prefab used to instantiate AudioSources.")]
    public AudioSource audioSourcePrefab;

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

        // Make the audiosource a singleton using Ben's singeloton script
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

    /// <summary>
    /// Play a sound by pulling a source from the pool and returning it after the sounds duration
    /// </summary>
    /// <param name="clip"> The sound file e.g. (.wav) </param>
    /// <param name="groupName"> Name of the audio mixer group it should be played in, SFX or Music </param>
    /// <param name="position"> Where the sound should play from in the game world </param>
    public void PlaySound(AudioClip clip, AudioMixerGroupName groupName, Vector3 position = default)
    {
        // Pull an audio source from the pool, used to save memory of creating and deleting sources
        AudioSource audioSource = audioSourcePool.Get();
        // Set input variables
        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(groupName);
        audioSource.Play();
        // Return source to pool when sound finishes playing
        StartCoroutine(ReturnAudioSourceAfterPlaying(audioSource));

    }

    /// <summary>
    /// Play a music track on a pre made audio source
    /// </summary>
    /// <param name="clip"> The sound file e.g. (.wav) </param>
    /// <param name="audioSource"> The audio source passed in by the music player script </param>
    public void PlayMusic(AudioClip clip, ref AudioSource audioSource)
    {
        audioSource.clip = clip;
        // loops track infinitly and sets to muted by default, music player will unmute when necessary
        audioSource.loop = true;
        audioSource.mute = true;
        audioSource.outputAudioMixerGroup = AudioManager.instance.GetAudioMixerGroup(AudioMixerGroupName.Music);
        audioSource.Play();
    }

    /// <summary>
    /// Returns an audio source to the pool, this is used to save memory
    /// </summary>
    /// <param name="audioSource"> source to be returned to pool </param>
    /// <returns> return in coroutine is used to make script wait a  number of seconds </returns>
    private IEnumerator ReturnAudioSourceAfterPlaying(AudioSource audioSource)
    {
        // Waits the length of the audio clip
        if (audioSource.outputAudioMixerGroup != GetAudioMixerGroup(AudioMixerGroupName.Music))
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        // returns source to pool
        audioSourcePool.ReturnToPool(audioSource);
    }
}
