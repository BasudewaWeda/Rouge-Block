using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public LayerMask ground;

    private float moveHorizontal;

    public float moveSpeed;
    public float accel;
    public float deaccel;
    public float velPower;

    public bool isJumping;
    public bool isGrounded;

    public float jumpForce;
    public bool jumpPressed;
    public float gravityScale;
    public float fallGravityMultiplier;
    public float jumpCutMultiplier;

    public AudioSource ac;
    public AudioClip jumpSound;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpPressed = true;
            ac.PlayOneShot(jumpSound); // Play jump sound (not in function because function gets called multiple times and sound would play alot of times in one jump)
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpPressed = false;
        }
    }

    void FixedUpdate()
    {
        // Running
        float targetSpeed = moveHorizontal * moveSpeed;
            // Calculate direction and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
            // Calculate difference between current speed and desired speed
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deaccel;
            // Change accel rate depending on situation
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            // Apply accel to speed dif, set power to increase accel with higher speeds
            // Multiply by sign to keep direction
        rb.AddForce(movement * Vector2.right);

        // Jumping
        if (jumpPressed && !isJumping) // Jump when space is pressed and currently not jumping
        {
            Jump();
        }

        if (isGrounded) // Set jumping to false when touching ground                                                                                
        {
            isJumping = false;
        }

        if (rb.velocity.y > 0 && !jumpPressed) // Jump cut implementation
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        if (rb.velocity.y < 0) // Fall Gravity
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
    }
}
