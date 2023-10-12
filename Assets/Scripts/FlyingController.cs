using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    [Header("Flying Stats")]
    [Tooltip("How much of the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    public float lift = 135f;

    private float throttle;
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

    Rigidbody rb;
    [SerializeField] Transform propella;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
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
        if (Input.GetKey(KeyCode.Space))  throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttle;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update() 
    {
        HandleInputs();
        if (propella)
            propella.Rotate(Vector3.forward * throttle);
    }

    private void FixedUpdate() 
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }
}
