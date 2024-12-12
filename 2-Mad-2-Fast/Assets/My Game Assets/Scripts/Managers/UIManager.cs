using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PedalUIParameters[] pUiP = new PedalUIParameters[2];


    public static UIManager instance;

    void Start()
    {
  
    }

    private void Awake()
    {
        instance = Singleton<UIManager>.get();
    }


    public void UpdatePedalUI(bool leftNext, bool rightNext, int playerNo)
    {
        if (leftNext)
        {
            pUiP[playerNo - 1].pedalImages[0].sprite = pUiP[playerNo - 1].pedalActive;
        }
        else
        {
            pUiP[playerNo - 1].pedalImages[0].sprite = pUiP[playerNo - 1].pedalInactive;
        }

        if (rightNext)
        {
            pUiP[playerNo - 1].pedalImages[1].sprite = pUiP[playerNo - 1].pedalActive;
        }
        else
        {
            pUiP[playerNo - 1].pedalImages[1].sprite = pUiP[playerNo - 1].pedalInactive;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
