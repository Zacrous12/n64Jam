using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    [SerializeField] public bool isActive;
    public int timeLastActive;
    private MeshRenderer mr;
    private bool fade;
    private Vector3 startPos;

    void Start()
    {
        isActive = true;
        timeLastActive = 0;
        mr = GetComponent<MeshRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        if(!isActive)
        {
            timeLastActive++;
            mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, mr.material.color.a - 0.01f);
        } 
        else if(mr.material.color.a < 1)
        {
            mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, mr.material.color.a + 0.01f);
        }
        if(timeLastActive > 350)
        {
            isActive = true;
            timeLastActive = 0;
        }
        // if(mr.material.color.a <= 0) mr.SetActive(false);
        // else mr.SetActive(true);

        transform.position = startPos;
        transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
        transform.position = transform.position + new Vector3(0, 2, 0);
    }

    public void Deactivate(){
        isActive = false;
        fade = true;
        //StartCoroutine(Fade());
    }

    public void Activate(){
        isActive = true;
        fade = false;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(2);
    }
}
