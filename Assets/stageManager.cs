using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageManager : MonoBehaviour
{
    private bool ghostMode;
    public GameObject ghostStage;
    public GameObject realStage;

    void Start()
    {
        ghostMode = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ghostMode = !ghostMode;
            SwapStage();
        }
    }
    private void SwapStage()
    {
        if(ghostMode)
        {
            ghostStage.SetActive(true);
            realStage.SetActive(false);
        }
        else
        {
            ghostStage.SetActive(false);
            realStage.SetActive(true);
        }
    }
}
