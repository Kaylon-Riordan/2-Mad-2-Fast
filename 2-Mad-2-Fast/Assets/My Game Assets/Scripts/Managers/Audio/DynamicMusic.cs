using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.SpriteMask;

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

        AudioManager.instance.PlayMusic(main, ref mainSource);
        AudioManager.instance.PlayMusic(medium, ref mediumSource);
        AudioManager.instance.PlayMusic(fast, ref fastSource);
        AudioManager.instance.PlayMusic(final, ref finalSource);
        mainSource.mute = false;

        PlayerPhysics.changeSpeed += CheckSpeed;
        LapManager.lastLap += LastLap;
    }

    void CheckSpeed()
    {
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

    void LastLap()
    {
        finalSource.mute = false;
    }

    public void MuteAll()
    {
        mainSource.mute = true;
        mediumSource.mute = true;
        fastSource.mute = true;
        finalSource.mute = true;
    }
}