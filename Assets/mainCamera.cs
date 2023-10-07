using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    public GameObject player;
    public float smoothing;

    void Start()
    {
        player = GameObject.Find("Player");
        smoothing = 0.125f;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, -7);
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (player.transform.position.x - transform.position.x) * .1f;
        float tempY = (player.transform.position.y - transform.position.y) * .1f;
        transform.position = new Vector3(tempX, tempY + 5, -7);
    }
}
