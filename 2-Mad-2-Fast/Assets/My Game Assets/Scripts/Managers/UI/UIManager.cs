using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [HideInInspector]
    public PedalSwitcher pedalUI;
    [HideInInspector]
    public TiltOMeter tiltUI;
    [HideInInspector]
    public WinScreen winScreen;

    private void Awake()
    {
        instance = Singleton<UIManager>.get();
        pedalUI = GetComponent<PedalSwitcher>();
        tiltUI = GetComponent<TiltOMeter>();
        winScreen = GetComponent<WinScreen>();
    }
}
