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

    private void OnDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
}
