using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public TMP_Text playerHP;
    public static UIManager I;
    void Awake()
    {
        I = this;
    }
    public void SetPlayerHP(float hp)
    {
        playerHP.text = $"Health: {hp}";
    }
}