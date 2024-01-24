using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    private Rigidbody2D rb; // Rigidbody of the player
    [SerializeField] private FixedJoystick joystick; // Reference to the joystick

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get the horizontal and vertical input from the joystick
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        // Create a new vector for the movement direction
        Vector2 moveVector = new Vector2(moveX, moveY);

        // Apply the movement to the player's Rigidbody
        rb.velocity = moveVector * speed;
    }
}
