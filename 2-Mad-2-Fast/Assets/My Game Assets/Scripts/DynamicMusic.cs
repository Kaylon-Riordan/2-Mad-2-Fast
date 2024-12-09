using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    Boolean always = true;
    Boolean mediumBool = false;
    Boolean fastBool = false;
    Boolean finalBool = false;

    void Start()
    {
        always = true;
        mediumBool = false;
        fastBool = false;
        finalBool = false;
        AudioManager.instance.PlayMusic(main, AudioMixerGroupName.Music,ref always, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (PlayerInput player in PlayerManager.instance.players)
        //{
        //    if (player.gameObject.GetComponent<PlayerPhysics>().speed >= player.gameObject.GetComponent<PlayerCollider>().fastSpeed)
        //    {
        //        fastBool = true;
        //        AudioManager.instance.PlayMusic(fast, AudioMixerGroupName.Music, ref fastBool, transform.position);
        //        while (player.gameObject.GetComponent<PlayerPhysics>().speed >= player.gameObject.GetComponent<PlayerCollider>().fastSpeed) {}
        //        fastBool = false;
        //    }
        //    else if (player.gameObject.GetComponent<PlayerPhysics>().speed >= player.gameObject.GetComponent<PlayerCollider>().moderateSpeed)
        //    {
        //        mediumBool = true;
        //        AudioManager.instance.PlayMusic(medium, AudioMixerGroupName.Music, ref mediumBool, transform.position);
        //        while (player.gameObject.GetComponent<PlayerPhysics>().speed >= player.gameObject.GetComponent<PlayerCollider>().moderateSpeed) {}
        //        mediumBool = false;
        //    }
        //}
    }

}
