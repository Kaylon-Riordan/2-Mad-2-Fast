using UnityEngine;

public class TiltOMeter : MonoBehaviour
{
    public RectTransform[] arrowPivots = new RectTransform[2]; 

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        for (int i = 0; i < arrowPivots.Length; i++)
        {
            arrowPivots[i].localEulerAngles = new Vector3(0, 0, PlayerManager.instance.players[i].GetComponent<PlayerTilting>().tiltShown.z);
        }
    }
}