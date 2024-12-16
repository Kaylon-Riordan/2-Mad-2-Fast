using UnityEngine;
using UnityEngine.UI;

public class PedalSwitcher : MonoBehaviour
{
    /// <summary>
    /// canvasImage1/2 - the Image where the Pedals will be on the canvas
    /// </summary>
    public Image[] canvasImages = new Image[2];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PlayerNo"></param>
    /// <param name="leftNext"></param>
    public void SetPedalSprite(int PlayerNo, bool leftNext)
    {
        if (!leftNext)
        {
            canvasImages[PlayerNo - 1].sprite = Resources.Load<Sprite>("UI/Pedal Left Pressed");
        } 
        
        else
        {
            canvasImages[PlayerNo - 1].sprite = Resources.Load<Sprite>("UI/Pedal Right Pressed");
        }
    }
}