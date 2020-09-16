using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public float maxHealth = 100f;

    public float maxShield = 200f;
    
    private float health;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            
            onHealthChange?.Invoke();
        }
    }

    private float armor;
    public float Armor
    {
        get => armor;
        set
        {
            armor = value;
            
            onArmorChange?.Invoke();
        }
    }

    public Action onHealthChange;
    public Action onArmorChange;

    public void SetUp()
    {
        Health = maxHealth;
        Armor = maxShield;
    }

    public void TakeDamage(float damageAmount)
    {
        if (Armor > 0)
        {
            Armor -= damageAmount;
        }
        else
        {
            Health -= damageAmount;
        }
    }
}
