using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHeadAI : BaseAI
{
    private void Start()
    {
        List<BTNode> myTree = new List<BTNode>()
        {
            Seq_CombatSequence()
        };
        
        PopulateBtNodes(myTree);
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        yield return new WaitForSeconds(1f);
        
        Vector3 explosionPos = transform.position;

        var colliders = new Collider[32];

        var size = Physics.OverlapSphereNonAlloc(explosionPos, 20, colliders, attackMask);

        for (var i = 0; i < size; i++)
        {
            Collider hit = colliders[i];
            Rigidbody rb = hit.attachedRigidbody;
            
            Debug.Log(hit.name);

            BaseEntity baseEntity = hit.GetComponent<BaseEntity>();

            if (baseEntity == self)
            {
                continue;
            }

            if (rb != null)
            {
                rb.AddExplosionForce(1500, explosionPos, 20, 3.0F);
            }

            var damageable = hit.GetComponent<Damageable>();
            
            if (damageable != null)
                damageable.TakeDamage(unitData.damage);

            var playerManager = hit.GetComponent<PlayerManager>();
            
            if (playerManager != null)
                PlayerEvents.Current.Shake();

            attackTimer = 0;
        }
        
        yield return new WaitForSeconds(1);

        isAttacking = false;
    }

    public override void Attack()
    {
        attackTimer += Time.deltaTime;
        
        Debug.Log("Preparing attack");

        if (!(attackTimer >= unitData.fireRate)) return;
        
        if (isAttacking) return;

        StartCoroutine(AttackSequence());
        
        Debug.Log("attack");

        
    }
}
