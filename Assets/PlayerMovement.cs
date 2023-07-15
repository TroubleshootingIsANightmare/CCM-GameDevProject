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
    public float maxGroundSpeed;

    [Header("Air")]
    public float airSpeedMultiplier;
    public float maxAirSpeed;

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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, ground);



        MyInput();
        SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
            maxSpeed = maxGroundSpeed;
        } else
        {
            rb.drag = 0;
            maxSpeed = maxAirSpeed;
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
        float fallSpeed = rb.velocity.y;
        
        
        if(rb.velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVel = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, fallSpeed, limitedVel.z);
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
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime, ForceMode.Impulse);
        }
        if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * airSpeedMultiplier, ForceMode.Impulse);
        }
    }
}
  
