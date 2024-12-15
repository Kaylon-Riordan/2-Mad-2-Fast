using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputPreferences", menuName = "PedalFury/Input Preferences")]

/// <summary>
/// Input preferences for both players, each preference stored in an array. Index[0] player 1, index[1] player 2
/// </summary>
public class InputPreferences : ScriptableObject
{
    //Determines which side will be chosen for one handed control scheme

    public bool[] leftHand = new bool[2];

    [SerializeField]
    public InputMethod[] inputMethods = new InputMethod[2];

    [SerializeField]
    public ControlScheme[] controlSchemes = new ControlScheme[2];
}