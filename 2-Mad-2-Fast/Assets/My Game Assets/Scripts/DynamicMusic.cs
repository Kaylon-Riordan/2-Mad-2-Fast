using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.SpriteMask;

public class DynamicMusic : MonoBehaviour
{
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
    void Start()
    {
        mainSource = new GameObject().AddComponent<AudioSource>();
        mediumSource = new GameObject().AddComponent<AudioSource>();
        fastSource = new GameObject().AddComponent<AudioSource>();
        finalSource = new GameObject().AddComponent<AudioSource>();

        AudioManager.instance.PlayMusic(main, ref mainSource);
        AudioManager.instance.PlayMusic(medium, ref mediumSource);
        AudioManager.instance.PlayMusic(fast, ref fastSource);
        AudioManager.instance.PlayMusic(final, ref finalSource);
        mainSource.mute = false;
    }

    //Update is called once per frame
    void Update()
    {
        StartCoroutine(checkSpeed(PlayerManager.instance.players[0].gameObject, PlayerManager.instance.players[1].gameObject));
    }

    private IEnumerator checkSpeed(GameObject player1, GameObject player2)
    {
        if (player1.GetComponent<PlayerPhysics>().speed >= player1.GetComponent<PlayerCollider>().fastSpeed || player2.GetComponent<PlayerPhysics>().speed >= player2.GetComponent<PlayerCollider>().fastSpeed)
        {
            fastSource.mute = false;
            yield return new WaitUntil(() => (player1.GetComponent<PlayerPhysics>().speed < player1.GetComponent<PlayerCollider>().fastSpeed && player2.GetComponent<PlayerPhysics>().speed < player2.GetComponent<PlayerCollider>().fastSpeed));
            fastSource.mute = true;
        }
        else if (player1.GetComponent<PlayerPhysics>().speed >= player1.GetComponent<PlayerCollider>().moderateSpeed || player2.GetComponent<PlayerPhysics>().speed >= player2.GetComponent<PlayerCollider>().moderateSpeed)
        {
            mediumSource.mute = false;
            yield return new WaitUntil(() => ((player1.GetComponent<PlayerPhysics>().speed < player1.GetComponent<PlayerCollider>().moderateSpeed && player2.GetComponent<PlayerPhysics>().speed < player2.GetComponent<PlayerCollider>().moderateSpeed) || 
                (player1.GetComponent<PlayerPhysics>().speed >= player1.GetComponent<PlayerCollider>().fastSpeed || player2.GetComponent<PlayerPhysics>().speed >= player2.GetComponent<PlayerCollider>().fastSpeed)));
            mediumSource.mute = true;
        }
    }
}
