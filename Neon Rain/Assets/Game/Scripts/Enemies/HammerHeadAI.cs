using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHeadAI : BaseAI
{   
    private BTSequence engageEnemySequence;
    private BTSelector targetInRangeSelector;
    private BTSelector heardNoiseSelector;
    private BTSelector hasWaypointSelector;
    private BTSequence officerAliveSequence;
    private BTSequence investigateSequence;
    private BTSequence patrolSequence;

    private float m_shootingTimer;
    

    private void Awake()
    {
    }

    private void Start()
    { 
        targetInRangeSelector = new BTSelector(new List<BTNode>()
        {
            new EnemyInRangeCondition(this),
            new MoveToTask(this)
        });
        
        engageEnemySequence = new BTSequence(new List<BTNode>()
        {
            new EnemySpottedCondition(this),
            // new GainAwarenessTask(this),
            // new FillAwarenessMeterTask(this),
            // new IsAwareCondition(this),
            // new SetDangerStateTask(this),
            new FaceTargetTask(this),
            targetInRangeSelector,
            new AttackTask(this)
        });
        
        investigateSequence = new BTSequence(new List<BTNode>()
        {
            new HeardNoiseCondition(this),
            new GainAwarenessTask(this),
            new FillAwarenessMeterTask(this),
            new IsAwareCondition(this),
            new SetAlertStateTask(this),
            new MoveToTask(this)
        });
        
        officerAliveSequence = new BTSequence(new List<BTNode>()
        {
            new ResistFleeCondition(this),
            new OfficerDeadCondition(this),
            new ShouldFleeCondition(this),
            new FleeTask(this)
        });
        
        rootAI = new BTSelector(new List<BTNode>()
        {
            new IsActiveCondition(this),
            //officerAliveSequence,
            engageEnemySequence,
            //investigateSequence,
            //patrolSequence
        });
    }
    
    public override void Attack()
    {
        m_shootingTimer += Time.deltaTime;

        if (!(m_shootingTimer >= this.unitData.fireRate)) return;
        
        Vector3 explosionPos = transform.position;
        
        var colliders = new Collider[32];
        
        var size = Physics.OverlapSphereNonAlloc(explosionPos, 20,  colliders, m_attackLayerMask);

        for (var i = 0; i < size; i++)
        {
            Collider hit = colliders[i];
            Rigidbody rb = hit.attachedRigidbody;
            
            BaseEntity baseEntity = hit.GetComponent<BaseEntity>();

            if (baseEntity == myEntity)
            {
                continue;
            }

            if (rb != null)
            {
                rb.AddExplosionForce(1500, explosionPos, 20, 3.0F);
            }

            var damageable = hit.GetComponent<Damageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(unitData.damage);
            }
                
        }
        
        m_shootingTimer = 0;
        
    }
}
