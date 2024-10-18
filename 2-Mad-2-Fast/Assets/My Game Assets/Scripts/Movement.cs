using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputAction playerControls;

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private CharacterController characterContoller;
    Vector2 inputDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterContoller = GetComponent<CharacterController>();
    }

    // Fixed update is used for movement so that the players speed isn't affected by framerate
    void FixedUpdate()
    {
        // Creates a variable that stores a float based on the players input on the horizontal and vertical axis, raw means there is no smoothing so is better fordigital input like keyboard
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        //float verticalInput = Input.GetAxisRaw("Vertical");
        inputDirection = playerControls.ReadValue<Vector2>();

        // Changes the rigid bodyie's velocity to be the detected horizontal input multiplied by the player's speed allowing it to move left and right
        Vector3 movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        float speed = inputMagnitude * maxSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        //transform.Translate(movementDirection * magnitude * Time.deltaTime, Space.World);
        characterContoller.SimpleMove(movementDirection * speed);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}