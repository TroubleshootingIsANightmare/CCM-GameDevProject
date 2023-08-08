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

    [Header("Keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode punchKey = KeyCode.F;

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
    public bool readyToJump, canPunch, punchable;
    public float jumpForce;
    public float maxSpeed;
    public float punchDamage, punchCooldown, punchRange;
    public LayerMask punchLayer;
    public Animator animator;
    public float speed;
    public float jumpCooldown;
    public float horizontalInput, verticalInput;
    Vector3 moveDirection;


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
        


        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, ground);
        var emit = speedParticles.emission;
        Vector3 velocityForward = rb.velocity;
        velocityForward.y = 0f;
        velocityForward.Normalize();

        float angle = Vector3.Angle(orientation.forward, velocityForward);

        float threshold = 30f;

        if (angle <= threshold && Input.GetAxisRaw("Vertical") > 0 && grounded || !grounded && angle <= threshold)
        {
            pcamera.fieldOfView = fov + rb.velocity.magnitude / 3;
            emit.rateOverTime = rb.velocity.magnitude;
        } else
        {
            pcamera.fieldOfView = Mathf.Lerp(pcamera.fieldOfView, fov, 0.1f);
            emit.rateOverTime = 0;
        }


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
        bool punching = Input.GetKeyDown(punchKey);

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump= false;
            Jump();
            Invoke("ResetJump", jumpCooldown);
        }
        if(punching && canPunch)
        {
            Punch();
        }
    }

    void Punch()
    {

        canPunch = false;

        Invoke("ResetPunch", punchCooldown);
    }

    void ResetPunch()
    {
   

        animator.SetBool("Punching", false);
        canPunch = true;
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
  
