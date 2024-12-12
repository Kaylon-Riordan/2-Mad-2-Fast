using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PedalUIParameters", menuName = "PedalFury/PedalUIParamters")]
public class PedalUIParameters : ScriptableObject
{
    [SerializeField]
    public Image[] pedalImages;

    [SerializeField]
    public Sprite pedalInactive;

    [SerializeField]
    public Sprite pedalActive;
}