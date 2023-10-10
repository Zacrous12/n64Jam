using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    public GameObject player;
    public float smoothing;
    private GameObject[] chars;
    private int currentChar;
    private int numChars;

    void Start()
    {
        numChars = 2;
        currentChar = 0;
        player = GameObject.Find("Player");
        smoothing = 0.015f;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, -7);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Make the camera lerp less jittery
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y + 5.0f, -7);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}
