using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private float startX;
    public float walkSpeed;
    public float walkRange;
    private bool isRight;
    
    void Start()
    {
        isRight = false;
        startX = transform.position.x;    
    }

    void Update()
    {
        if((transform.position.x < (startX - walkRange))) isRight = true;
        else if(transform.position.x > (startX + walkRange)) isRight = false;
        if(isRight){
            Vector3 newPos = new Vector3(transform.position.x + walkSpeed, transform.position.y, transform.position.z); 
            transform.position = newPos;
        } 
        else 
        {
            Vector3 newPos = new Vector3(transform.position.x - walkSpeed, transform.position.y, transform.position.z); 
            transform.position = newPos;
        }
    }
}
