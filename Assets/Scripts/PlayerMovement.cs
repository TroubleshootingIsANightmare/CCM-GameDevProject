using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody rb;
    public Transform orientation;
    public Camera pcamera;
    public float fov;
    public float playerHeight;
    public Transform player;

    [Header("Keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftShift;

    [Header("Grounded")]
    public LayerMask ground;
    public float counterThreshold;
    public bool grounded;
    public float groundDrag;
    public float maxGroundSpeed;

    [Header("Air")]
    public float airSpeedMultiplier;
    public float maxAirSpeed;

    [Header("Movement")]
    public bool readyToJump, readyToSlide, jumping, sliding;
    public float jumpForce, slideForce;
    public float counterMovement;
    public float maxSpeed;
    public float multiplier = 1f, vMult = 1f;
    public float speed;
    public float jumpCooldown, slideCooldown;
    public float horizontalInput, verticalInput;
    Vector3 playerScale = new Vector3(1f,1f,1f);
    Vector3 slideScale = new Vector3(1f, 0.5f, 1f);

    Vector3 moveDirection;
    Vector3 inputDirection;

    [Header("Particles")]
    public ParticleSystem speedParticles;

    void Start()
    {
        //Grab the rigidbody from the player
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }
    void Update()
    {

        //Print the velocity
        Debug.Log(rb.velocity.magnitude);

        //Extra Gravity
        rb.AddForce(0f, -10f * Time.deltaTime, 0f, ForceMode.Force);

        //Detect the ground using ray
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, ground);

        //Particles rate of emission based on rb speed
        var emit = speedParticles.emission;
        Vector3 velocityForward = rb.velocity;
        velocityForward.y = 0f;
        velocityForward.Normalize();

        //Float to check if facing in the same direction as moving
        float angle = Vector3.Angle(orientation.forward, velocityForward);

        //Number of degrees which count as facing forward
        float threshold = 30f;

        //Detect if angle is less than threshold
        if (angle <= threshold && Input.GetAxisRaw("Vertical") > 0 && grounded || !grounded && angle <= threshold) emit.rateOverTime = rb.velocity.magnitude;
        else emit.rateOverTime = 0;








    }
    void FixedUpdate()
    {
        //Call move every single tick, regardless of frame rate
        Move();
        //Call input 
        MyInput();
    }

    void Jump()
    {

        jumping = true;
        rb.AddForce(Vector2.up * jumpForce * 1.5f);
        rb.AddForce(moveDirection * jumpForce * 0.025f);

    }

    void ResetJump()
    {
        readyToJump = true;
    }




    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }

        if (Input.GetKey(slideKey) && readyToSlide && !sliding && horizontalInput != 0 || Input.GetKey(slideKey) && readyToSlide && !sliding && verticalInput != 0)
        {
            readyToSlide = false;
            sliding = true;
            inputDirection = orientation.forward * verticalInput + horizontalInput * orientation.right;
            
        }
        if (!Input.GetKey(slideKey)) sliding = false;

        if (sliding) Slide(); player.localScale = slideScale;
        if(!sliding) player.localScale = playerScale;

    }

    void Slide()
    {
        rb.AddForce(inputDirection  * slideForce * Time.deltaTime, ForceMode.Impulse);
        Invoke("ResetSlide", slideCooldown);
    }

    void ResetSlide()
    {
        sliding = false;
        readyToSlide = true;
    }


    void Move()
    {
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        CounterMovement(horizontalInput, verticalInput, mag);
        moveDirection = vMult * orientation.forward * verticalInput + orientation.right * horizontalInput;


        if (horizontalInput > 0 && xMag > maxSpeed) horizontalInput = 0;
        if (horizontalInput < 0 && xMag < -maxSpeed) horizontalInput = 0;
        if (verticalInput > 0 && yMag > maxSpeed) verticalInput = 0;
        if (verticalInput < 0 && yMag < -maxSpeed) verticalInput = 0;
        

        if (grounded && !sliding)
        {
            jumping = false;
            maxSpeed = maxGroundSpeed;
            multiplier = 1f;
            vMult = 1f;
        }
        if(sliding)
        {
            vMult = 0f;
            multiplier = 0f;
        }
        if(sliding && grounded)
        {
            rb.AddForce(0,-3000f,0, ForceMode.Impulse);
        }
        if (!grounded && !sliding)
        {
            maxSpeed = maxAirSpeed;
            multiplier = 0.5f;
            vMult = 0.5f;
        }

        rb.AddForce(orientation.right * horizontalInput * speed * multiplier * Time.deltaTime);
        rb.AddForce(vMult * orientation.forward * verticalInput * speed * multiplier * Time.deltaTime);
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
        if (!grounded || sliding) return;

        if (horizontalInput == 0 && verticalInput == 0) rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, 0), 0.5f);
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

