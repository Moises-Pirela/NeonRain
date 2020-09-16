using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionEvents : MonoBehaviour
{
    PlayerManager playerManager;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            PlayerEvents.Current.isGrappling = false;
        }
    }
}
