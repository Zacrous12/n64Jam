using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // transform.position = startPos;
        // transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
        // transform.position = transform.position + new Vector3(0, 2, 0);
    }
}
