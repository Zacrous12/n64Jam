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
    public float jumpForce;

    private float moveInput;

    private Rigidbody rb;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;

    private Animator anim;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    private int extraJumpsValue;

    private void MovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        rb.velocity = new Vector3(moveInput.x * speed, moveInput.y * speed, rb.velocity.y);
    }

    private void MoveCanceled(InputAction.CallbackContext context)
    {

    }
    private void SmashPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Smash");
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
        move.performed += MovePerformed;
        move.canceled += MoveCanceled;

        smash.performed += SmashPerformed;
        smash.canceled += SmashCanceled;

        special.performed += SpecialPerformed;
        special.canceled += SpecialCanceled;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

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

    }

    void Update()
    {

    }
}
