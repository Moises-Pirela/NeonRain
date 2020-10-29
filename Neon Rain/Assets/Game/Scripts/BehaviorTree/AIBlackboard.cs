using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AIBlackboard : MonoBehaviour
{
    public static AIBlackboard _current;

    public GameObject player;

    public Waypoint[] waypoints;

    private void Awake()
    {
        _current = this;
        
    }

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public float GetPlayerExposure()
    {
        var playerExposure = player.GetComponent<PlayerManager>().Exposure;

        return playerExposure;
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return player.transform.rotation;
    }

    public float GetPlayerHealth()
    {
        var playerHealth = player.GetComponent<BaseEntity>().Health;

        return playerHealth;
    }
}
