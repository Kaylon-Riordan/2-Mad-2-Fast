using UnityEngine;
using UnityEngine.UI;

public class PedalSwitcher : MonoBehaviour
{
    public Image canvasImage1; // Image component on the canvas
    public Sprite pedalL1;     // Sprite for PedalL
    public Sprite pedalR1;     // Sprite for PedalR

    public Image canvasImage2;
    public Sprite pedalL2;    // Sprite for PedalL for the second set of pedals
    public Sprite pedalR2;    // Sprite for PedalR for the second set of pedals

    void Update()
    {
        // Check for the "J" key press
        if (Input.GetKeyDown(KeyCode.J))
        {
            SetPedalSprite(pedalL1);
        }

        // Check for the "K" key press
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetPedalSprite(pedalR1);
        }
    }

    // Method to set the sprite of the canvas Image
    void SetPedalSprite(Sprite newSprite)
    {
        if (canvasImage1 != null && newSprite != null)
        {
            canvasImage1.sprite = newSprite;
        }
    }
}