using UnityEngine;
using UnityEngine.UI;

public class PedalSwitcher : MonoBehaviour
{
    /// <summary>
    /// canvasImage1/2 - the Image where the Pedals will be on the canvas
    /// pedalL/R - Sprites that show either If the left or right pedal is pressed
    /// pedalLeft/Right Key - the left and Right Key for both racers
    /// </summary>
    public Image canvasImage1;
    public Image canvasImage2;
    public Sprite pedalL;     
    public Sprite pedalR;     

    public KeyCode pedal1LeftKey = KeyCode.J;
    public KeyCode pedal1RightKey = KeyCode.K;

    public KeyCode pedal2LeftKey = KeyCode.Mouse0;
    public KeyCode pedal2RightKey = KeyCode.Mouse1;

    void Update()
    {
        if (Input.GetKeyDown(pedal1LeftKey))
        {
            SetPedalSprite(canvasImage1, pedalL);
        }

        if (Input.GetKeyDown(pedal1RightKey))
        {
            SetPedalSprite(canvasImage1, pedalR);
        }

        if (Input.GetKeyDown(pedal2LeftKey))
        {
            SetPedalSprite(canvasImage2, pedalL);
        }

        if (Input.GetKeyDown(pedal2RightKey))
        {
            SetPedalSprite(canvasImage2, pedalR);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasImage"></param>
    /// The Canvas Image used in this method for the pedals
    /// <param name="newSprite"></param>
    /// the sprites to change the canvas to 
    void SetPedalSprite(Image canvasImage, Sprite newSprite)
    {
        if (canvasImage != null && newSprite != null)
        {
            canvasImage.sprite = newSprite;
        }
    }
}