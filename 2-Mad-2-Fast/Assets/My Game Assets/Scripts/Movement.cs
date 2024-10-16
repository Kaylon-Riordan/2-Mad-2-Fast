using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    private CharacterController characterContoller;

    // Start is called before the first frame update
    void Start()
    {
        characterContoller = GetComponent<CharacterController>();
    }

    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        // Creates a variable that stores a float based on the players input on the horizontal and vertical axis, raw means there is no smoothing so is better fordigital input like keyboard
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Changes the rigid bodyie's velocity to be the detected horizontal input multiplied by the player's speed allowing it to move left and right
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        //transform.Translate(movementDirection * magnitude * Time.deltaTime, Space.World);
        characterContoller.SimpleMove(movementDirection * magnitude);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
