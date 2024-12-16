using UnityEngine;
using UnityEngine.UI;

public class PedalSwitcher : MonoBehaviour
{
    /// <summary>
    /// canvasImage1/2 - the Image where the Pedals will be on the canvas
    /// </summary>
    public Image[] canvasImages = new Image[2];

    /// <summary>
    /// Sets pedal sprite according to what pedal is next for which player
    /// </summary>
    /// <param name="PlayerNo">Int 1 or 2 representing Player</param>
    /// <param name="leftNext">Whether or not the left pedal is the next pedal</param>
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