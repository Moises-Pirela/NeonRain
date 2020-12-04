using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : BaseEntity
{
    private void Awake()
    {
        SetUp();
        onHealthChange += OnDeath;
    }

    public void Respawn()
    {
        SetUp();
    }

    private void OnDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        
        if (Health > 0)
        {
            if (hitEffect != null)
            {
                var effect = Instantiate(hitEffect, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
                Destroy(effect, 1f);
            }
        }
        else
        {
            if (deathEffect != null)
            {
                var effect = Instantiate(deathEffect, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
                Destroy(effect, 1f);
            }
        }
    }
}
