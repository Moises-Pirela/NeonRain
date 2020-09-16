using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseEntity
{
    [SerializeField] private float armorRegenAmount;
    [SerializeField] private float armorRegenTimer;
    [SerializeField] private float armorRegenTime;

    private void Awake()
    {
        SetUp();
    }

    private void Update()
    {
        if ((Armor > 0) && (Armor < maxShield))
        {
            armorRegenTimer += Time.deltaTime;

            if (!(armorRegenTimer >= armorRegenTime)) return;
            
            Armor += armorRegenAmount;
            armorRegenTimer = 0;    
        }
    }
}
