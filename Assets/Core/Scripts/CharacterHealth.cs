using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class CharacterHealth : MonoBehaviour
{
    public bool IsPlayer = false;
    public float MaxHealth;
    public float CurrentHealth;
    public ExampleCharacterController PlayerController;
    void Start()
    {
        CurrentHealth = MaxHealth;
        if (IsPlayer)
            UIManager.I.SetPlayerHP(CurrentHealth);
    }
    private void OnTriggerEnter(Collider other)
    { 
        Enemy enemy = other.GetComponent<Enemy>();
        ExampleCharacterController player = other.GetComponent<ExampleCharacterController>();
        if (enemy)
        {
            if (PlayerController)
            {
                if (!PlayerController.IsSprinting)
                {
                    IncrementHealth(-enemy.HarmAmount); 
                }
            }
        }
        else if (player)
        {
            if (player.IsSprinting)
                IncrementHealth(-1f); 
        }
    }
    private void IncrementHealth(float amount)
    {
        CurrentHealth += amount;
        
        // Is dead
        if (CurrentHealth <= 0)
        {
            if (IsPlayer)
            {

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        // Take damage
        else if (amount < 0)
        {
            if (IsPlayer)
            {
                
            }
            else
            {

            }
        }
        // Heal
        else if (amount > 0)
        {
            if (IsPlayer)
            {

            }
            else
            {

            }
        }
        if (IsPlayer) 
        {
            UIManager.I.SetPlayerHP(CurrentHealth);
        }
    }
}