using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputPreferences", menuName = "PedalFury/Input Preferences")]
public class InputPreferences : ScriptableObject
{
    //[SerializeField] // Determines which side will be chosen for one handed control scheme
    //public List<bool> leftHand = new List<bool>();

    public bool[] leftHand = new bool[2];

    [SerializeField]
    public InputMethod[] inputMethods = new InputMethod[2];

    [SerializeField]
    public ControlScheme[] controlSchemes = new ControlScheme[2];
}