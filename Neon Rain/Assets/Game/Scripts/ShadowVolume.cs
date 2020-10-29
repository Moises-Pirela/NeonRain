using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowVolume : MonoBehaviour
{
    public float exposure;

    private float playerInitialExposure;

    private bool playerHidden = false;
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();
        
        if (!other) return;
        
        if(playerHidden) return;

        player.Exposure -= exposure;

        playerHidden = true;
        
        Debug.LogFormat("Player exposure {0}", player.Exposure);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();
        
        if (!other) return;

        if (!playerHidden) return;

        player.Exposure += exposure;

        playerHidden = false;
    }
}
