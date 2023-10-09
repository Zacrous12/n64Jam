using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    [SerializeField] public bool isActive;
    public int timeLastActive;
    private MeshRenderer mr;
    void Start()
    {
        isActive = true;
        timeLastActive = 0;
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(!isActive){
            mr.enabled = false;
            timeLastActive++;
        }
        if(timeLastActive > 200){
            isActive = true;
            timeLastActive = 0;
            mr.enabled = true;
        }
    }

    public void Deactivate(){
        isActive = false;
    }

    public void Activate(){
        isActive = true;
    }
}
