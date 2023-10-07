using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public PlayerInput playerActions;
    public InputAction move;
    public InputAction smash;
    public InputAction special;

    public float speed;
    public float dashSpeed;
    public float jumpSpeed;
    public float jumpForce;
    public float airDrift;

    private float moveInput;

    private Rigidbody rb;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;
    public bool isHoldDown;

    private Animator anim;

    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    private int extraJumpsValue;

    private void MoveStarted(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if(isGrounded) rb.AddForce(new Vector3(moveInput.x * dashSpeed, 0, rb.velocity.z));
        else {
            if(extraJumpsValue > 0)
            {
                rb.velocity = new Vector3(moveInput.x, 0, rb.velocity.z);
                extraJumpsValue--;
            }
        }
        
    }
    
    private void MovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if(isGrounded){
            rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, rb.velocity.z);
        }
        else {
            rb.velocity = new Vector3(moveInput.x * airDrift, rb.velocity.y, rb.velocity.z);
        }
        if(moveInput.y < 0) isHoldDown = true;
        else isHoldDown = false;
    }

    private void MoveCanceled(InputAction.CallbackContext context)
    {
        isHoldDown = false;
    }

    private void SmashPerformed(InputAction.CallbackContext context)
    {
        
    }

    private void SmashCanceled(InputAction.CallbackContext context)
    {

    }

    private void SpecialPerformed(InputAction.CallbackContext context)
    {

    }

    private void SpecialCanceled(InputAction.CallbackContext context)
    {

    }

    void Awake()
    {
        playerActions = GetComponent<PlayerInput>();

        move = playerActions.actions.FindAction("Move", true);
        move.started += MoveStarted;
        move.performed += MovePerformed;
        move.canceled += MoveCanceled;

        smash.performed += SmashPerformed;
        smash.canceled += SmashCanceled;

        special.performed += SpecialPerformed;
        special.canceled += SpecialCanceled;

        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();

        extraJumpsValue = extraJumps;
    }

    void OnEnable()
    {
        move.Enable();
        smash.Enable();
        special.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        smash.Disable();
        special.Disable();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(rb.position, checkRadius, whatIsGround);
        Debug.Log(Physics.CheckSphere(rb.position, checkRadius, whatIsGround));

        if(isGrounded == true)
        {
            extraJumpsValue = extraJumps;
        }
    }

    void Update()
    {
    }
}
