using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System;

public class FlyingController : MonoBehaviour
{

    public bool IsFlying { get; private set; }
    public KinematicCharacterMotor Motor;
    public ExampleCharacterController CharacterController;
    [Header("Flying Stats")]
    [Tooltip("How much of the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float initialThrust = 50f;
    public float responsiveness = 10f;
    public float lift = 135f;

    private float throttle;
    private float upThrust;
    private float roll;
    private float pitch;
    private float yaw;
    
    private float responseModifier 
    {
        get 
        { 
            return (rb.mass / 10f) * responsiveness;
        }
    }
    public float ResponseModifier => responseModifier;

    Rigidbody rb;
    [SerializeField] Transform propella;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetFlyingState(bool flying)
    {
        IsFlying = flying;
    }
    private void FireBullet()
    {
        //GameObject bul = ObjectPool.i.GetObject("Bullet");
        //bul.transform.position = transform.position;
        //bul.transform.rotation = transform.rotation;
        //BulletManager.i.AddBullet(bul, transform.forward);
    }

    private void HandleInputs() 
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetButtonDown("Fire1")) 
        {
            FireBullet();
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (upThrust <= initialThrust) upThrust = initialThrust; 
            else upThrust += throttleIncrement;
        }
        if (Input.GetKey(KeyCode.Space))  
        {
            if (throttle <= initialThrust) throttle = initialThrust; 
            else throttle += throttleIncrement;
        }
        if (Input.GetKey(KeyCode.LeftControl)) 
        {
            Debug.Log("Dethrust");
            upThrust -= upThrust;
            throttle -= throttle;
        }
        throttle = Mathf.Clamp(throttle, 0, 100f);
        upThrust = Mathf.Clamp(throttle, 0, 100f);
    }

    private void Update() 
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!IsFlying)
                CharacterController.TransitionToState(CharacterState.Flying);
            else
                CharacterController.TransitionToState(CharacterState.Default);      
            //throttle = initialThrust;
            //Motor.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            CharacterController.TransitionToState(CharacterState.Default);
            //Motor.enabled = true;
        }
        if (IsFlying)
        {
            HandleInputs();
            if (propella)
                propella.Rotate(Vector3.forward * throttle);
        }
        */
    }

    private void FixedUpdate() 
    {
        if (IsFlying)
        {
            /*
            //rb.AddForce(transform.up * maxThrust * upThrust);
            //rb.AddForce(transform.forward * maxThrust * throttle);
            rb.AddTorque(transform.up * yaw * responseModifier);
            rb.AddTorque(transform.right * pitch * responseModifier);
            rb.AddTorque(-transform.forward * roll * responseModifier);

            rb.AddForce(Vector3.forward * rb.velocity.magnitude * lift);
            */
            
        }
    }
}
