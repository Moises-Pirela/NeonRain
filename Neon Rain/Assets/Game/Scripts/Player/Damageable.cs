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

    public GameObject hitEffect;
    public GameObject deathEffect;

    public GameObject spriteMesh;

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

            armor = Mathf.Clamp(value, 0, maxShield) ;

            onArmorChange?.Invoke(armor, initialArmor);
        }
    }

    public Action onHealthChange;
    public Action<float, float> onArmorChange;
    public Action onDeath;

    private void Awake()
    {
        SetUp();
    }

    protected void SetUp()
    {
        Health = maxHealth;
        Armor = maxShield;
    }

    public virtual void TakeDamage(float damageAmount)
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

    // public IEnumerator Die()
    // {
    //     // deathEffect.gameObject.SetActive(true);
    //     // deathEffect.Play();
    //     // deathEffect.transform.parent = null;
    //     // hitEffect.transform.parent = null;
    //     // spriteMesh.SetActive(false);
    //     //
    //     // yield return new WaitForSeconds(1f);
    //     //
    //     // onDeath?.Invoke();
    // }
}