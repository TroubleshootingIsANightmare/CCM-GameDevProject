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
    public float multiplier = 1f;
    [SerializeField] Vector3 moveDirection;
    public float fallingSpeedMultiplier;
    [SerializeField] Vector3 moveForce;
    public float slideMaxSpeed;
    public float normMaxSpeed;
    public float dashDistance = 5f; // Adjust the distance of the dash as needed
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics.Raycast(orientation.position, Vector3.down, 2f * 0.5f + 0.2f, whatIsGround);
        Move();
        Debug.Log(rb.velocity.magnitude);



    }

    void Move()
    {

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * y + orientation.right * x;
        moveForce = moveDirection.normalized * speed * 10f * multiplier;
        if (!isSliding) {
            rb.AddForce(moveForce, ForceMode.Force);

        } else if (isSliding)
        {
            rb.drag = 0;
        }

        if (grounded && !isSliding)
        {
            rb.drag = groundDrag;
            multiplier = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Dash();
        }

        if (!grounded)
        {
            rb.drag = airDrag;
            multiplier = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Slide");
            Slide();
            slideTimer = 0f;
            maxSpeed = slideMaxSpeed;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {

            isSliding = false;
            player.localScale = new Vector3(1f, 1f, 1f);
            maxSpeed = normMaxSpeed;
        }
        TimerControl();
        CounterMovement();
        if (canJump && Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("Jump");
        }

    }

    void CounterMovement()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !isSliding && grounded)
        {
            // Slow down the player's velocity
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, counterForce * Time.deltaTime);
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, 0), counterForce / 3f * Time.deltaTime);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        jumpTimer = 0f;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void Slide()
    {
        player.localScale = slideScale;
        isSliding = true;
        rb.drag = slideDrag;
        rb.AddForce(moveForce * slideSpeed);

    }

    void TimerControl()
    {
        jumpTimer += 1f;
        slideTimer += 1f;
        if (jumpTimer > jumpCooldown && grounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (slideTimer > slideCooldown)
        {
            canSlide = true;
        } else
        {
            canSlide = false;
        }
    }

    void Dash()
    {
        // Calculate the dash direction based on the player's current movement direction
        Vector3 dashDirection = moveDirection.normalized;


    // Apply the dash force to the Rigidbody
    rb.AddForce(dashDirection* dashDistance, ForceMode.Impulse);
    }
        
 }
  
