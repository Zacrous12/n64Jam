using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public enum AbilityType
{
    Default,
    Flying,
}
public class AbilityManager : MonoBehaviour
{
    public bool FlyingUnlocked = false;
    public ExampleCharacterController PlayerController;
    private void OnTriggerEnter(Collider other)
    { 
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            if (PlayerController.IsSprinting)
            {
                switch(enemy.AbilityType)
                {
                    case (AbilityType.Default):
                    {
                        break;
                    }
                    case (AbilityType.Flying):
                    {
                        FlyingUnlocked = true;
                        break;
                    }
                }
            }
            else
            {
                
            }
        }
    }
}