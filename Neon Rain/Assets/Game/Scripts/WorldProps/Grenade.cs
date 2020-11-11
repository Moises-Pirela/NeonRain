using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive
{
    [SerializeField] private Rigidbody rb;
    public float launchForce;

    public void Start()
    {
        Launch(transform.forward);
    }

    public void Launch(Vector3 forceDirection)
    {
        rb.AddForce(forceDirection * launchForce, ForceMode.Impulse);

        StartCoroutine(InitiateExplode());
    }
    
    public IEnumerator InitiateExplode()
    {
        yield return new WaitForSeconds(5f);
        
        base.Explode();
    }

    public override void TakeDamage(float damageAmount)
    {
        //Do nothing
    }
}
