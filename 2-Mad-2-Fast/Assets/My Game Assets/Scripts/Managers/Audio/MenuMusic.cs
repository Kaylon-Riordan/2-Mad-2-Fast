using UnityEngine;

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

    // Copy of the Dynamic music script in the level, changed to work for the menu
    void Start()
    {
        menuSource = new GameObject().AddComponent<AudioSource>();
        optionsSource = new GameObject().AddComponent<AudioSource>();

        AudioManager.instance.PlayMusic(menu, ref menuSource);
        AudioManager.instance.PlayMusic(options, ref optionsSource);
        menuSource.mute = false;
    }

    // Methods are called from the unity event system, attatched to the canvas in the menu scene
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