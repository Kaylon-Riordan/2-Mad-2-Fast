using UnityEngine;

//  Learned how to use delegates for observer https://youtu.be/J01z1F-du-E
public class DynamicMusic : MonoBehaviour
{
    public static DynamicMusic instance;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip main;
    [SerializeField]
    private AudioClip medium;
    [SerializeField]
    private AudioClip fast;
    [SerializeField]
    private AudioClip final;

    AudioSource mainSource;
    AudioSource mediumSource;
    AudioSource fastSource;
    AudioSource finalSource;

    private PlayerPhysics pp1;
    private PlayerPhysics pp2;
    private LapManager lm;

    private void Awake()
    {
        instance = Singleton<DynamicMusic>.get();
    }

    void Start()
    {
        pp1 = PlayerManager.instance.players[0].GetComponent<PlayerPhysics>();
        pp2 = PlayerManager.instance.players[1].GetComponent<PlayerPhysics>();
        lm = LapManager.instance;

        mainSource = new GameObject().AddComponent<AudioSource>();
        mediumSource = new GameObject().AddComponent<AudioSource>();
        fastSource = new GameObject().AddComponent<AudioSource>();
        finalSource = new GameObject().AddComponent<AudioSource>();

        // Play all the songs at the start so they are all in sync
        // They are all muted by default and will be unmuted by the methods in this calss when appropriate
        AudioManager.instance.PlayMusic(main, ref mainSource);
        AudioManager.instance.PlayMusic(medium, ref mediumSource);
        AudioManager.instance.PlayMusic(fast, ref fastSource);
        AudioManager.instance.PlayMusic(final, ref finalSource);
        // Play the main section of the track from the start
        mainSource.mute = false;

        // Adds these methods to the matching delegates
        // Delegates are like events inside their parent scripts that update when a specific thing happens
        // Change speed delegate signals when the player moves speed bracket inside the physics script
        // Last lap delegate signals whenever a player starts their last lap inside the lap manager script
        // These delegates then trigger the matching method inside this class
        PlayerPhysics.changeSpeed += CheckSpeed;
        LapManager.lastLap += LastLap;
    }

    /// <summary>
    /// Is called when player changes into a different speed bracket, updates music to match.
    /// </summary>
    void CheckSpeed()
    {
        // Check if one player is fast, then medium, then set the appropriate tracks to mute or unmute
        if (pp1.speedBracket == Speed.Fast || pp2.speedBracket == Speed.Fast)
        {
            fastSource.mute = false;
            mediumSource.mute = true;
        }
        else if (pp1.speedBracket == Speed.Medium || pp2.speedBracket == Speed.Medium)
        {
            fastSource.mute = true;
            mediumSource.mute = false;
        }
        else
        {
            fastSource.mute = true;
            mediumSource.mute = true;
        }
    }

    /// <summary>
    /// Unmutes the last lap music when last lap starts
    /// </summary>
    void LastLap()
    {
        finalSource.mute = false;
    }

    /// <summary>
    /// Mutes all music tracks
    /// </summary>
    public void MuteAll()
    {
        mainSource.mute = true;
        mediumSource.mute = true;
        fastSource.mute = true;
        finalSource.mute = true;
    }
}