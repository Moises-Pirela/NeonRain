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
            CombatSequence(),
            //WanderSequence()
        };
        
        PopulateBtNodes(myTree);
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        if (agent.enabled)
            agent.isStopped = true;

        yield return new WaitForSeconds(1f);
        
        Vector3 explosionPos = transform.position;

        var colliders = new Collider[32];

        var size = Physics.OverlapSphereNonAlloc(explosionPos, 10, colliders, attackMask);

        for (var i = 0; i < size; i++)
        {
            Collider hit = colliders[i];
            Rigidbody rb = hit.attachedRigidbody;

            BaseEntity baseEntity = hit.GetComponent<BaseEntity>();

            if (baseEntity != null)
            {
                if (baseEntity == self || baseEntity.faction == self.faction)
                {
                    continue;
                }    
            }
            

            if (rb != null && baseEntity == null)
            {
                rb.AddExplosionForce(3000, explosionPos, 10, 1.0F);
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
        
        
        if (agent.enabled)
            agent.isStopped = false;

        isAttacking = false;
    }

    public override void Attack()
    {   
        attackTimer += Time.deltaTime;
        
        if (!(attackTimer >= unitData.fireRate)) return;
        
        if (isAttacking) return;

        StartCoroutine(AttackSequence());

        
    }
}
