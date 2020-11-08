using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHeadFSM : StateMachine
{
    public override void AttackAction()
    {
        if (Vector3.Distance(transform.position, CurrentTarget.transform.position) <= unitData.attackRange)
        {
            attackTimer += Time.deltaTime;
        
            Debug.Log("Preparing attack");

            if (!(attackTimer >= unitData.fireRate)) return;
        
            if (isAttacking) return;

            StartCoroutine(AttackSequence());   
        }
        else
        {
            if (!isAttacking)
                currentState = States.CHASING;
        }
    }

    public override void ChaseAction()
    {
        if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > unitData.attackRange)
        {
            agent.SetDestination(CurrentTarget.transform.position);
        }
        else
        {
            currentState = States.ATTACKING;
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        agent.isStopped = true;

        yield return new WaitForSeconds(1f);
        
        Debug.Log("attack");
        
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
        
        agent.isStopped = false;
        

        isAttacking = false;
    }
}
