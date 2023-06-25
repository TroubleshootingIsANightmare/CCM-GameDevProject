using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform orientation;
    public LayerMask whatIsGround;
    public bool grounded;
    public float speed;
    public float maxSpeed;
    public float groundDrag;
    public float jumpForce;
    public float slideSpeed;
    public float x, y;
    public float slideDrag;
    public float airDrag;
    public float counterForce;
    public bool isSliding, canJump, canSlide;
    public float jumpTimer, jumpCooldown;
    public float slideTimer, slideCooldown;
    Vector3 slideScale = new Vector3(1f, 0.5f, 1f);
    public Transform player;
    [SerializeField] Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics.Raycast(orientation.position, Vector3.down, 2f * 0.5f + 0.2f, whatIsGround);
        Move();
        rb.AddForce(Vector3.down * 10f * Time.deltaTime);
    }

    void Move()
    {

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        moveDirection = orientation.forward * y + orientation.right * x;
        if (!isSliding) {
            rb.AddForce(moveDirection.normalized * speed * 10f);
        }

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        if (!grounded)
        {
            rb.drag = airDrag;
        }
        if (Input.GetButton("Slide"))
        {
            Debug.Log("Slide");
        }
        TimerControl();
        CounterMovement();
        if(canJump && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    void CounterMovement()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            // Slow down the player's velocity
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, counterForce * Time.deltaTime);
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            x = 0f;
            y = 0f;
        }
    }

    void Jump()
    {

    }

    void Slide()
    {
        player.localScale = slideScale;
        isSliding = true;
        rb.drag = slideDrag;
        rb.AddForce(moveDirection.normalized * slideSpeed);
    }

    void TimerControl()
    {
        jumpTimer += 1f;
        slideTimer += 1f;
        if (jumpTimer > jumpCooldown)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if(slideTimer > slideCooldown)
        {
            canSlide = true;
        } else
        {
            canSlide = false;
        }
    }
}
