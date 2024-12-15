using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIltOMeter : MonoBehaviour
{
    /// <summary>
    /// ArrowPivot 1/2 - Pivot of Arrow 1/2 (Parent in Inspector)
    /// RotationSpeed = How fast it goes
    /// Max Angle - Max it can go 90d (And then in code the limit is -90d)
    /// ResetSpeed - how fast it returns to the center pivot
    /// arrow1/2 left/right key - The key used to determine which way to move (left or right)
    /// CurrentRot 1/2 = Updates as you turn
    /// </summary>
    public RectTransform arrowPivot1; 
    public RectTransform arrowPivot2;

    public float rotationSpeed = 50f;
    public float maxRotationAngle = 90f;
    public float resetSpeed = 200f;

    public KeyCode arrow1LeftKey = KeyCode.A;
    public KeyCode arrow1RightKey = KeyCode.D;

    public KeyCode arrow2LeftKey = KeyCode.LeftArrow;
    public KeyCode arrow2RightKey = KeyCode.RightArrow;

    private float currentRotation1 = 0f;
    private float currentRotation2 = 0f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// Checks which input is needed for the movement e.g A/D for left right Player1 Controls
    /// <param name="currentRotation"></param>
    /// Takes in the currentPosition when calculating where to move the arrow along the circle
    /// <param name="arrow"></param>
    /// The pivot point that the arrows moves around
    private void ArrowRotation(float input, ref float currentRotation, RectTransform arrow)
    {
        if (input != 0)
        {
            // Calculate the new rotation angle
            float rotationAmount = input * rotationSpeed * Time.deltaTime;
            float newRotation = currentRotation + rotationAmount;

            // Clamp the new rotation within bounds
            newRotation = Mathf.Clamp(newRotation, -maxRotationAngle, maxRotationAngle);

            // Apply the rotation to the arrow
            arrow.localEulerAngles = new Vector3(0, 0, newRotation);
            currentRotation = newRotation;
        }
       
        else //else is when input is not happening and proceeds to reset
        {
            // Smoothly reset the arrow to the base position
            float resetAmount = resetSpeed * Time.deltaTime;

            if (currentRotation > 0) //when arrow is between 1-90
            {
                currentRotation = Mathf.Max(0, currentRotation - resetAmount);
            }
            else if (currentRotation < 0) //when arrow is between -1 - -90
            {
                currentRotation = Mathf.Min(0, currentRotation + resetAmount);
            }

            // Apply the reset rotation to the arrow using Eulers (METHOD FOUND ON FORUMS)
            arrow.localEulerAngles = new Vector3(0, 0, currentRotation);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (arrowPivot1 != null)
        {
            float input = 0;
            if (Input.GetKey(arrow1LeftKey)) input = -1;
            if (Input.GetKey(arrow1RightKey)) input = 1;
            ArrowRotation(-Input.GetAxis("Horizontal"), ref currentRotation1, arrowPivot1); //for some reason the arrow was flipped and so I needed to -Input both of these methods
        }
        else
        {
            Debug.LogError("Arrow 1 is fake"); //if it doesn not exist
        }

        if (arrowPivot2 != null)
        {
            float input = 0;
            if (Input.GetKey(arrow2LeftKey)) input = -1;
            if (Input.GetKey(arrow2RightKey)) input = 1;
            ArrowRotation(-Input.GetAxis("HorizontalArrows"), ref currentRotation2, arrowPivot2);
        }
        else
        {
            Debug.LogError("Arrow 2 is fake"); //see comment above
        }

    }

}


