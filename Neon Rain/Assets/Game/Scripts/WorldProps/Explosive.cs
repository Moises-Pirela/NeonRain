using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosive : Damageable
{
    public bool canExplode;

    public ExplosiveData data;

    public LayerMask layerMask;

    public GameObject explosionEffect;

    public virtual void Explode()
    {
        var hitColliders = new Collider[32];
        var position = transform.position;
        var numColliders =  Physics.OverlapSphereNonAlloc(position, data.explosionRadius, hitColliders, layerMask);
        
        explosionEffect.SetActive(true);
        explosionEffect.transform.parent = null;
        
        for (var i = 0; i < numColliders; i++)
        {
            var currentCollider = hitColliders[i];
            
            if (!currentCollider) continue;
            
            if (currentCollider.GetComponent<BaseEntity>())
                currentCollider.GetComponent<BaseEntity>().TakeDamage(data.explosionDamage);
            
            if (!currentCollider.GetComponent<Rigidbody>()) continue;
            var direction = currentCollider.transform.position - transform.position;
            var rg = currentCollider.GetComponent<Rigidbody>();
            
            var playerManager = currentCollider.GetComponent<PlayerManager>();
            
            if (playerManager != null)
                PlayerEvents.Current.Shake();

            //rg.isKinematic = false;

            rg.AddExplosionForce(data.explosionForce, position, data.explosionRadius, 3.0F);
        }
        
        Destroy(explosionEffect,2f);
        Destroy(gameObject);
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        if (Health <= 0)
        {
            Explode();
        }
    }
}
