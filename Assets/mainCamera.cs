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
        player = GameObject.Find("Fastfaller");
        smoothing = 0.055f;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, -7);
    }

    void FixedUpdate()
    {
        // Make the camera lerp less jittery
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y + 5.0f, -7);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}
