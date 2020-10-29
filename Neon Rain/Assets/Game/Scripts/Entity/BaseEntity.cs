using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Faction
{
    NAZI,
    SONS_OF_LIBERTY,
    NONE
}

public enum EntityType
{
    AI,
    PLAYER
}

public abstract class BaseEntity : Damageable
{   
    public Faction faction;
    public EntityType entityType;

    public Animator animator;

    public void Start()
    {
        GameMaster._current.onRestartLevel += () =>
        {
            isDead = false;
            SetUp();
        };
    }
}
