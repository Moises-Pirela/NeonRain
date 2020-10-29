using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public float maxHealth = 100f;

    public float maxShield = 200f;
    
    [HideInInspector] public bool takingDamage = false;
    [HideInInspector] public bool isDead = false;
    
    private float health;
    public float Health
    {
        get => health;
        set
        {
            if (value < health)
                takingDamage = true;
            
            health = value;
            
            onHealthChange?.Invoke();

            if (health <= 0)
            {
                isDead = true;
                onDeath?.Invoke();
            }
        }
    }

    private float armor;
    public float Armor
    {
        get => armor;
        set
        {
            var initialArmor = armor;
            
            if (value < armor)
                takingDamage = true;
            
            armor = value;
            
            onArmorChange?.Invoke(armor , initialArmor);
        }
    }

    public Action onHealthChange;
    public Action<float, float> onArmorChange;
    public Action onDeath;

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
