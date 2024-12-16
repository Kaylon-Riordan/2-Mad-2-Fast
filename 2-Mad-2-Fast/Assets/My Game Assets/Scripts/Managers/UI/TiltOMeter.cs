using UnityEngine;

/// <summary>
/// Rotates each player's arrow pivot at the same angle found in each player's tiltshown
/// </summary>
public class TiltOMeter : MonoBehaviour
{
    public RectTransform[] arrowPivots = new RectTransform[2]; 


    void FixedUpdate()
    {
        for (int i = 0; i < arrowPivots.Length; i++)
        {
            arrowPivots[i].localEulerAngles = new Vector3(0, 0, PlayerManager.instance.players[i].GetComponent<PlayerTilting>().tiltShown.z);
        }
    }
}