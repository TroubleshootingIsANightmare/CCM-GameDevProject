using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody rb;
    public Transform orientation;
    public Camera pcamera;
    public float fov;
    public Animator animator;

    [Header("Inputs")]
    public float horizontalInput, verticalInput;


    [Header("Keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode punchKey = KeyCode.F;

    [Header("Grounded")]
    public LayerMask ground;
    public float counterThreshold;
    public float playerHeight;
    public bool grounded;
    public float maxGroundSpeed;

    [Header("Air")]
    public float airSpeedMultiplier;
    public float maxAirSpeed;

    [Header("Movement")]

    public float counterMovement;
    public float maxSpeed;
    public bool running;
    public float runSpeed;
    public bool moving;
    public bool launched;
    public Vector2 maxRunVel;
    public float speed;


    Vector3 moveDirection;

    [Header("Jump")]
    public bool readyToJump;
    public float jumpForce;
    public float jumpCooldown;

    [Header("Particles")]
    public ParticleSystem speedParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }
    void Update()
    {
        maxRunVel = new Vector2(rb.velocity.x, rb.velocity.z);
        Debug.Log(rb.velocity.magnitude);
        Debug.Log(maxRunVel);

        rb.AddForce(0f, -10f * Time.deltaTime, 0f, ForceMode.Force);
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, ground);
        var emit = speedParticles.emission;
        Vector3 velocityForward = rb.velocity;
        velocityForward.y = 0f;
        velocityForward.Normalize();

        float angle = Vector3.Angle(orientation.forward, velocityForward);

        float threshold = 30f;

        if (angle <= threshold && Input.GetAxisRaw("Vertical") > 0 && grounded || !grounded && angle <= threshold)
        {

            emit.rateOverTime = rb.velocity.magnitude;
        } else
        {

            emit.rateOverTime = 0;
        }


        MyInput();
        SpeedControl();

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
            rb.AddForce(-moveDirection * counterMovement * 1.5f);
        }
        if (grounded && moving && !running)
        {
            rb.AddForce(-moveDirection.normalized * counterMovement * 2f, ForceMode.Force);
        }
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump= false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }
        if(horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0 || verticalInput < 0 )
        {
            moving = true;
         
            if (maxRunVel.magnitude < runSpeed + 10)
            {
                running = true;
                launched = false;
            } else
            {
                launched = true;
                running = false;
            }
        } else
        {
            moving = false;
            running = false;
            launched = false;
            
        }
   
    }

    
    void Move()
    {
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        CounterMovement(horizontalInput, verticalInput, mag);
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded && !running && launched)
        {
            maxSpeed = maxGroundSpeed;
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime, ForceMode.Impulse);
        }



        if(!grounded && !running && launched)
        {
            maxSpeed = maxAirSpeed;
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * airSpeedMultiplier, ForceMode.Impulse);
        }

        if(running)
        {
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime, ForceMode.Impulse);
            maxSpeed = runSpeed;
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }


    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || moving) return;

       
        //Counter movement
        if (Mathf.Abs(mag.x) > counterThreshold && Mathf.Abs(x) < 0.05f || (mag.x < -counterThreshold && x > 0) || (mag.x > counterThreshold && x < 0))
        {
            rb.AddForce(speed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > counterThreshold && Mathf.Abs(y) < 0.05f || (mag.y < -counterThreshold && y > 0) || (mag.y > counterThreshold && y < 0))
        {
            rb.AddForce(speed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

}

