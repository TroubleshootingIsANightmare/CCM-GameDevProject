using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody rb;
    public Transform orientation;

    [Header("Keys")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Grounded")]
    public LayerMask ground;
    public float playerHeight;
    public bool grounded;
    public float groundDrag;

    [Header("Air")]
    float airSpeedMultiplier;

    [Header("Movement")]
    public bool readyToJump;
    public float jumpForce;
    public float maxSpeed;
    public float speed;
    public float jumpCooldown;
    public float horizontalInput, verticalInput;
    Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);



        MyInput();
        SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0;
        }

    }
    void FixedUpdate()
    {
        Move();
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    } 

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump)
        {
            readyToJump= false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }
    }

    void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Impulse);
        }
        if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f * airSpeedMultiplier, ForceMode.Impulse);
        }
    }
}
  
