using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputPreferenceReader : MonoBehaviour
{
    private OptionsMenu oM;
    private PlayerPanel[] playerPanels = new PlayerPanel[2];
    public InputPreferences ip;

    private void Awake()
    {
        oM = GetComponent<OptionsMenu>();
    }

    public void SavePreferences()
    {
        playerPanels[0] = oM.player1Panel;
        playerPanels[1] = oM.player2Panel;

        for (int i = 0; i < playerPanels.Length; i++)
        {
            ip.leftHand[i] = oM.player1Panel.getLeftHandToggle();
            ip.inputMethods[i] = playerPanels[i].getInputMethod();
            ip.controlSchemes[i] = playerPanels[i].getControlScheme();
        }
    }
}
