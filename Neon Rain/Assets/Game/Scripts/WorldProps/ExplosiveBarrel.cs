using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Explosive
{
    public GameObject fireStream;
    public bool exploding;
    
    public IEnumerator InitiateExplode()
    {
        exploding = true;
        fireStream.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        
        fireStream.SetActive(false);
        base.Explode();
    }

    public override void Explode()
    {
        if (!exploding)
            StartCoroutine(InitiateExplode());
    }

    public override void TakeDamage(float damageAmount)
    {
        var healthBonus = Health - damageAmount;
        
        if (healthBonus < 0 && exploding)
        {
            base.Explode();
        }
        
        base.TakeDamage(damageAmount);

    }
}
