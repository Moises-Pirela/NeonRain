using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseEntity
{
    [SerializeField] private float armorRegenAmount;
    [SerializeField] private float armorRegenTimer;
    [SerializeField] private float armorRegenTime;
    [SerializeField] private CharacterController _controller;
    private float exposure = 1;

    public float Exposure
    {
        get => exposure;
        set => exposure = Mathf.Clamp(value,0,1);
    }

    private void Awake()
    {
        SetUp();
    }

    private new void Start()
    {
        PlayerEvents.Current.onPlayerCrouch += () => { exposure = 0.5f; };
        PlayerEvents.Current.onPlayerStand += () => { exposure = 1; };
        GameMaster._current.onRestartLevel += () => { _controller.enabled = true; };
        PlayerEvents.Current.sprinting += () =>
        {
            DrainArmor(0.05f);
        };
        
        onDeath += () => { _controller.enabled = false; };
    }

    private void Update()
    {
        if ((Armor >= 0) && (Armor < maxShield))
        {
            armorRegenTimer += Time.deltaTime;

            if (!(armorRegenTimer >= armorRegenTime)) return;

            if (takingDamage)
            {
                takingDamage = false;
            }
            else
            { 
                Armor += armorRegenAmount;   
            }
            
            armorRegenTimer = 0;    
        }
    }

    public void DrainArmor(float drainAmount)
    {
        Armor -= drainAmount;
    }
}
