using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Rigidbody2D playerRigidbody;

    private Transform _transform;

    private float HorizontalMovement
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    private bool JumpButtonPressed
    {
        get { return Input.GetKeyDown(KeyCode.Space); }
    }

    private bool IsGrounded
    {
        get { return playerRigidbody.velocity.y == 0; }
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.freezeRotation = true;
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        HandleHorizontalMovement();
        HandleJumping();
	}

    private void HandleHorizontalMovement()
    {
        var newVelocity = playerRigidbody.velocity;
        newVelocity.x = HorizontalMovement * speed;
        playerRigidbody.velocity = newVelocity;
    }

    private void HandleJumping()
    {
        if (JumpButtonPressed && IsGrounded)
        {
            var up = _transform.TransformDirection(Vector2.up);
            playerRigidbody.AddForce(up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
