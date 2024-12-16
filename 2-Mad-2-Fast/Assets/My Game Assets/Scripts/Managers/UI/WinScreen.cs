using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    /// <summary>
    /// canvasImage1/2 - the Image where the Pedals will be on the canvas
    /// </summary>
    public Image[] canvasImages = new Image[2];

    /// <summary>
    /// Sets win screen message at the end of the game
    /// </summary>
    /// <param name="PlayerNo">Int 1 or 2 representing Player 1 or 2</param>
    /// <param name="finished">Whether the player finished the race</param>
    public void SetWinScreen(int PlayerNo, bool finished)
    {
        if (finished)
        {
            canvasImages[PlayerNo - 1].sprite = Resources.Load<Sprite>("UI/Winner");
        }
        else
        {
            canvasImages[PlayerNo - 1].sprite = Resources.Load<Sprite>("UI/Loser");
        }
    }
}