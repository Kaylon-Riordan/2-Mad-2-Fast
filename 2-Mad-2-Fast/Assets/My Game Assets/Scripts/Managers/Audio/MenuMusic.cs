using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
using static UnityEngine.SpriteMask;

//  Learned how to use delegates for observer https://youtu.be/J01z1F-du-E
public class MenuMusic : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip menu;
    [SerializeField]
    private AudioClip options;

    [HideInInspector]
    public AudioSource menuSource;
    AudioSource optionsSource;

    void Start()
    {
        menuSource = new GameObject().AddComponent<AudioSource>();
        optionsSource = new GameObject().AddComponent<AudioSource>();
        
        AudioManager.instance.PlayMusic(menu, ref menuSource);
        AudioManager.instance.PlayMusic(options, ref optionsSource);
        menuSource.mute = false;
    }

    public void MenuOff()
    {
        menuSource.mute = true;
    }
    public void OptionsOn()
    {
        optionsSource.mute = false;
    }
    public void OptionsOff()
    {
        optionsSource.mute = true;
    }
}