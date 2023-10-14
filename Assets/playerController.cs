using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float speed;
    public float dashSpeed;
    public float maxSpeed;
    public float slowRate;
    public int dashTimer;
    public float dodgeSpeed;
    private int dodgeTimer;
    public float jumpForce;
    public int jumpTimer;
    public float jumpModifier;
    public float jumpApexTime;
    public float wallJumpForce;
    public float airDrift;
    public float maxAirDrift;
    public float wellDrift;
    public float gravity;
    public float maxWellGravity;
    public int gravTimer;
    public int activeTimer;

    private Rigidbody rb;
    private ParticleSystem ps;
    private ParticleSystem trans;
    private Transform psObj;
    private mainCamera cam;
    private Vector3 spawn;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;
    private bool isDashing;
    private bool readyToDash;
    private bool isCrouching;
    private bool isDodging;
    private bool inWell;
    private bool isInvincible;
    private bool facingRight;
    public bool isHoldDown;
    private bool outOfWell;
    private bool nearWell;
    private bool jumpApex;
    private bool canWallJump;
    private bool wallRight;

    private Animator anim;

    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    private int extraJumpsValue;

    // Gravity related variables
    public float distFromWell;
    public float wellGravity;
    private GameObject[] wells;

    private void HandleMove(bool right)
    {
        if(isGrounded){
            if(!isDashing && readyToDash){
                if(right) rb.velocity = (new Vector3(dashSpeed, rb.velocity.y, 0));
                else rb.velocity = (new Vector3(-dashSpeed, rb.velocity.y, 0));
                speed = dashSpeed;
                isDashing = true;
                readyToDash = false;
                ps.Play();
            }
            if(right) rb.velocity = (new Vector3(speed, rb.velocity.y, 0));
            else rb.velocity = (new Vector3(-speed, rb.velocity.y, 0));
        }
        else {
            if(canWallJump){
                float y;
                if(rb.velocity.y < 0) y = 0;
                else y = rb.velocity.y;
                if(wallRight && !right)
                {
                    rb.AddForce(new Vector3(-airDrift, y, 0));
                }
                else if(wallRight && right)
                {
                    rb.velocity = new Vector3(rb.velocity.x, y, 0);
                }
                else if(!wallRight && !right)
                {
                    rb.velocity = new Vector3(rb.velocity.x, y, 0);
                }
                else if(!wallRight && right)
                {
                    rb.AddForce(new Vector3(-airDrift, y, 0));
                }
            }

            else
            {
                if(right) rb.velocity = new Vector3(airDrift, rb.velocity.y, rb.velocity.z);
                else rb.velocity = new Vector3(-airDrift, rb.velocity.y, rb.velocity.z);
            }
        }
    }

    private void JumpStarted()
    {
        if(isJumping) return;
        if(isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            StartCoroutine(Jump());
        }
        else
        {
            if(extraJumpsValue > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                rb.AddForce(new Vector3(0, jumpForce * jumpModifier, 0), ForceMode.Impulse);
                extraJumpsValue--;
                jumpModifier *= .8f;
            }
            StartCoroutine(Jump());
        }
        isJumping = true;
    }

    private void WallJumpStarted()
    {
        if(isGrounded || !canWallJump) return;
        canWallJump = false;
        ps.Play();
        if(wallRight) rb.AddForce(new Vector3(-jumpForce * .5f, jumpForce, 0), ForceMode.Impulse);
        else rb.AddForce(new Vector3(jumpForce * .5f, jumpForce, 0), ForceMode.Impulse);
        StartCoroutine(WallJump());
        isJumping = true;
    }

    private void HandleDuck()
    {
        isHoldDown = true;
        if(isGrounded && !isCrouching)
        {
            transform.localScale = new Vector3(1, .5f, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y - .25f, transform.position.z);
            rb.AddForce(new Vector3(0, -.25f, 0), ForceMode.Impulse);
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

    IEnumerator lagDodge()
    {
        RaycastHit hit;
        bool part = false;
        yield return new WaitForSeconds(0.1f);
        if(Physics.Raycast(rb.position + new Vector3(0,-2.0f,0), transform.TransformDirection(Vector3.down), out hit, checkRadius, whatIsGround)) part = true;
        if(Physics.Raycast(rb.position + new Vector3(0,-1.0f,0), transform.TransformDirection(Vector3.up), out hit, checkRadius * 2, whatIsGround) || (rb.position.y < 1.0f)) part = true;
        if(part) yield break;
        rb.velocity = new Vector3(0, 0, 0);
        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(0.4f);
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator Jump()
    {
        jumpApex = false;
        yield return new WaitForSeconds(jumpApexTime);
        jumpApex = true;
    }

    IEnumerator WallJump()
    {
        for (int i = 0; i < 20; i++)
        {
            if(wallRight) rb.AddForce(new Vector3(-jumpForce * 0.5f, wallJumpForce, 0));
            else rb.AddForce(new Vector3(jumpForce * 0.5f, wallJumpForce, 0));
            yield return new WaitForSeconds(.012f);
        }
        canWallJump = false;
        jumpApex = false;
        yield return new WaitForSeconds(jumpApexTime * .75f);
        jumpApex = true;
    }

    private void HandleDodge()
    {
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
                rb.velocity = (new Vector3(0, 0, 0));
                rb.AddForce(new Vector3(dodgeSpeed, 0, 0), ForceMode.Impulse);
                if(Input.GetKey(KeyCode.W)) rb.velocity += (new Vector3(dodgeSpeed, dodgeSpeed, 0));
                else if(Input.GetKey(KeyCode.S)) rb.velocity += (new Vector3(dodgeSpeed, -dodgeSpeed, 0));
            }
            else if(Input.GetKey(KeyCode.A))
            {
                rb.velocity = (new Vector3(0, 0, 0));
                rb.AddForce(new Vector3(-dodgeSpeed, 0, 0), ForceMode.Impulse);
                rb.velocity += (new Vector3(-dodgeSpeed, 0, 0));
                if(Input.GetKey(KeyCode.W)) rb.velocity += (new Vector3(-dodgeSpeed, dodgeSpeed, 0));
                else if(Input.GetKey(KeyCode.S)) rb.velocity += (new Vector3(-dodgeSpeed, -dodgeSpeed, 0));
            }
            else if(Input.GetKey(KeyCode.W))
            {
                rb.velocity += (new Vector3(0, dodgeSpeed, 0));
            }
            else if(Input.GetKey(KeyCode.S))
            {
                rb.velocity += (new Vector3(0, -dodgeSpeed, 0));
            }
            isDodging = true;
            ps.Play();
            StartCoroutine(lagDodge());
        }
    }

    void Awake()
    {
        whatIsGround = LayerMask.GetMask("Ground");

        trans = GameObject.Find("trans").GetComponent<ParticleSystem>();
        ps = GetComponentInChildren<ParticleSystem>();
        psObj = GetComponentInChildren<Transform>();
        cam = GameObject.Find("Main Camera").GetComponent<mainCamera>();
        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();

        isJumping = false;
        isDodging = false;
        readyToDash = true;
        extraJumpsValue = extraJumps;

        outOfWell = true;
        nearWell = false;
        wells = GameObject.FindGameObjectsWithTag("Well");
        spawn = transform.position;
    }

    void Start()
    {
        trans.Play();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        isGrounded = (Physics.Raycast(rb.position, transform.TransformDirection(Vector3.down), out hit, checkRadius, whatIsGround));

        if(isJumping)
        {
            jumpTimer++;
            if(jumpTimer > 10)
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
            if(dodgeTimer > 120)
            {
                isDodging = false;
                dodgeTimer = 0;
            }
        }

        if(isGrounded == true)
        {
            extraJumpsValue = extraJumps;
            jumpModifier = .65f;
            gravTimer = 0;
            isDodging = false;
            canWallJump = false;
            outOfWell = true; // change where this is
        } 
        else if(jumpApex) 
        {
            rb.AddForce(new Vector3(0,-50,0),ForceMode.Acceleration);
        }
        else{
            rb.AddForce(new Vector3(0,5,0));
        }

        if(nearWell) airDrift = wellDrift;
        else airDrift = maxAirDrift;

        if(speed > maxSpeed) speed -= .1f; 
         
        nearWell = false;

        for(int i = 0; i < wells.Length; i++)
        {
            bool isWellActive = wells[i].GetComponent<Well>().isActive;
            if((Vector3.Distance(wells[i].transform.position, transform.position) < distFromWell) && isWellActive) {
                rb.velocity += wellGravity * Time.fixedTime * (wells[i].transform.position - transform.position);
                inWell = true;
                nearWell = true;
                gravTimer++;
            } 
            else if(!nearWell) inWell = false;

            // if((Vector3.Distance(wells[i].transform.position, transform.position) < (distFromWell * 0.5f))) 
            // {
            //     wellGravity = 0;
            //     outOfWell = false;
            //     nearWell = true;
            //     wells[i].GetComponent<Well>().Deactivate();
            // }
            // else wellGravity = maxWellGravity;

            if(gravTimer > activeTimer)
            {
                wellGravity = 0;
                outOfWell = false;
                gravTimer = 0;
                nearWell = true;
                wells[i].GetComponent<Well>().Deactivate();
            }
        }

        if(!isDodging) rb.AddForce(new Vector3(0, gravity, 0));
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
        else if(facingRight && rb.velocity.x > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x - slowRate, rb.velocity.y, 0);
        }
        else if(!facingRight && rb.velocity.x < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x + slowRate, rb.velocity.y, 0);
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            if(!facingRight) psObj.Rotate(0,180,0);
            facingRight = true;
        }
        else if(Input.GetKeyDown(KeyCode.A)) {
            if(facingRight) psObj.Rotate(0,-180,0);
            facingRight = false;
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            readyToDash = true;
            isDashing = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(canWallJump) WallJumpStarted();
            else JumpStarted();
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
            // HandleDodge();
        }

        if(Input.GetKey(KeyCode.K))
        {
            SpecialPerformed();
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            if(this.gameObject.name == "Fastfaller" || this.gameObject.name == "Fastfaller(Clone)")
            {
                Instantiate(Resources.Load<GameObject>("Ghost"), transform.position, transform.rotation);
                cam.player = GameObject.Find("Ghost(Clone)");
            } 
            else
            {
                Instantiate(Resources.Load<GameObject>("Fastfaller"), transform.position, transform.rotation);
                cam.player = GameObject.Find("Fastfaller(Clone)");
            }
            Destroy(this.gameObject);

            // When we have multiple characters
            // for(int i = 0; i < charList.Length; i++)
            // {
            //     if(charList[i].active == true)
            //     {
            //         charList[i].SetActive(false);
            //         if(i == charList.Length - 1) charList[0].SetActive(true);
            //         else charList[i + 1].SetActive(true);
            //         break;
            //     }
            // }
        }
    }

    private void OnCollisionEnter(Collision obj)
    {
        
        if(obj.collider.tag == "Death" || obj.collider.tag == "Enemy") 
        {
            transform.position = spawn;
            rb.velocity = new Vector3(0, 0, 0);
        }

        if(obj.collider.tag == "Checkpoint")
        {
            spawn = obj.gameObject.transform.position;
            // if(this.gameObject.name == "Ghost" || this.gameObject.name == "Ghost(Clone)")
            // {
            //     Instantiate(Resources.Load<GameObject>("Fastfaller"), transform.position, transform.rotation);
            //     cam.player = GameObject.Find("Player(Clone)");
            // } 
        }
    }

    private void OnCollisionStay(Collision obj)
    {
        if(obj.collider.tag == "Wall")
        {
            canWallJump = true;
            if(obj.collider.transform.position.x > transform.position.x) wallRight = true;
            else wallRight = false;
        } 
    }
}
