using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public int dashTimer;
    public float dodgeSpeed;
    private int dodgeTimer;
    public float jumpForce;
    public int jumpTimer;
    private float jumpModifier;
    public float airDrift;
    public float gravity;

    private Rigidbody rb;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;
    private bool isDashing;
    private bool readyToDash;
    private bool isCrouching;
    private bool isDodging;
    public bool isHoldDown;

    private Animator anim;

    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    private int extraJumpsValue;

    private void HandleMove(bool right)
    {
        if(isGrounded){
            if(!isDashing && readyToDash){
                if(right) rb.velocity = (new Vector3(dashSpeed, rb.velocity.y, 0));
                else rb.velocity = (new Vector3(-dashSpeed, rb.velocity.y, 0));
                speed = dashSpeed;
                isDashing = true;
                readyToDash = false;
            }
            if(right) rb.velocity = (new Vector3(speed, rb.velocity.y, 0));
            else rb.velocity = (new Vector3(-speed, rb.velocity.y, 0));
        }
        else {
            if(right) rb.velocity = new Vector3(airDrift, rb.velocity.y, rb.velocity.z);
            else rb.velocity = new Vector3(-airDrift, rb.velocity.y, rb.velocity.z);
        }
    }

    private void JumpStarted()
    {
        if(isJumping) return;
        if(isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        else
        {
            if(extraJumpsValue > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce * jumpModifier, 0);
                extraJumpsValue--;
                jumpModifier *= .8f;
            }
        }
        isJumping = true;
    }

    private void HandleDuck()
    {
        isHoldDown = true;
        if(isGrounded && !isCrouching)
        {
            transform.localScale = new Vector3(1, .5f, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y - .25f, transform.position.z);
            rb.AddForce(new Vector3(0, -.75f, 0), ForceMode.Impulse);
            isCrouching = true;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, -jumpForce, 0);
        }
    }

    private void SpecialPerformed()
    {

    }

    private void HandleDodge()
    {
        Debug.Log("Dodge");
        if(isGrounded)
        {
            //BLOCK
            return;
        }
        else if(!isDodging)
        {
            // TODO: Make invincible
            if(Input.GetKey(KeyCode.D))
            {
                if(Input.GetKey(KeyCode.W)) rb.AddForce(new Vector3(dodgeSpeed, dodgeSpeed, rb.velocity.z), ForceMode.Impulse);
                else if(Input.GetKey(KeyCode.S)) rb.AddForce(new Vector3(dodgeSpeed, -dodgeSpeed, rb.velocity.z), ForceMode.Impulse);
                else rb.AddForce(new Vector3(dodgeSpeed, rb.velocity.y, rb.velocity.z), ForceMode.Impulse);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                if(Input.GetKey(KeyCode.W)) rb.AddForce(new Vector3(-dodgeSpeed, dodgeSpeed, rb.velocity.z), ForceMode.Impulse);
                else if(Input.GetKey(KeyCode.S)) rb.AddForce(new Vector3(-dodgeSpeed, -dodgeSpeed, rb.velocity.z), ForceMode.Impulse);
                else rb.AddForce(new Vector3(-dodgeSpeed, rb.velocity.y, rb.velocity.z), ForceMode.Impulse);
            }
            else if(Input.GetKey(KeyCode.W))
            {
                rb.AddForce(new Vector3(rb.velocity.x, dodgeSpeed, rb.velocity.z), ForceMode.Impulse);
            }
            isDodging = true;
        }
    }

    void Awake()
    {
        whatIsGround = LayerMask.GetMask("Ground");

        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();

        isJumping = false;
        isDodging = false;
        readyToDash = true;
        extraJumpsValue = extraJumps;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        RaycastHit hit;
        isGrounded = (Physics.Raycast(rb.position, transform.TransformDirection(Vector3.down), out hit, checkRadius, whatIsGround));

        if(isJumping)
        {
            jumpTimer++;
            if(jumpTimer > 20)
            {
                isJumping = false;
                jumpTimer = 0;
            }
        }

        if(isDashing)
        {
            dashTimer++;
            if(dashTimer > 25 && readyToDash)
            {
                isDashing = false;
                dashTimer = 0;
            }
        }

        if(isDodging)
        {
            dodgeTimer++;
            if(dodgeTimer > 20)
            {
                isDodging = false;
                dodgeTimer = 0;
            }
        }

        if(isGrounded == true)
        {
            extraJumpsValue = extraJumps;
            jumpModifier = .75f;
            isDodging = false;
        }

        if(speed > 4) speed -= .1f; 

        rb.AddForce(new Vector3(0, gravity, 0));
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            HandleMove(true);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            HandleMove(false);
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            readyToDash = true;
            isDashing = true;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            JumpStarted();
        }

        if(Input.GetKey(KeyCode.S))
        {
            HandleDuck();
        }
        if(!Input.GetKey(KeyCode.S))
        {
            isHoldDown = false;
            isCrouching = false;
            transform.localScale = new Vector3(1, 1, 1);
        }   

        if(Input.GetKey(KeyCode.J))
        {
            HandleDodge();
        }

        if(Input.GetKey(KeyCode.K))
        {
            SpecialPerformed();
        }
    }
}
