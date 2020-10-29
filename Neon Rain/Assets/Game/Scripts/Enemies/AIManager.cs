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
            gameObject.SetActive(false);
    }
}
