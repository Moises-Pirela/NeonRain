using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NaziTrooperAIController : BaseAI, IPatrol
{
    private BTSequence engageEnemySequence;
    private BTSelector targetInRangeSelector;
    private BTSelector heardNoiseSelector;
    private BTSelector hasWaypointSelector;
    private BTSequence officerAliveSequence;
    private BTSequence investigateSequence;
    private BTSequence patrolSequence;

    public List<Waypoint> patrolPoints;
    public Waypoint _currentWaypoint { get; set; }
    public List<Waypoint> _patrolPoints
    {
        get => patrolPoints;
        set => patrolPoints = value;
    }
    
    private void Start()
    {   
        hasWaypointSelector = new BTSelector(new List<BTNode>()
        {
            new HasWaypointTargetCondition(this),
            new SelectPatrolPointTask(this)
        });
        
        patrolSequence = new BTSequence(new List<BTNode>()
        {
            new IsPatrolCondition(this),
            hasWaypointSelector,
            new PatrolTask(this, this),
            new ReachedDestinationCondition(this),
            new SelectPatrolPointTask(this)
        });
        
        targetInRangeSelector = new BTSelector(new List<BTNode>()
        {
            new EnemyInRangeCondition(this),
            new MoveToTask(this)
        });

        engageEnemySequence = new BTSequence(new List<BTNode>()
        {
            new EnemySpottedCondition(this),
            new GainAwarenessTask(this),
            new FillAwarenessMeterTask(this),
            new IsAwareCondition(this),
            new SetDangerStateTask(this),
            new FaceTargetTask(this),
            targetInRangeSelector,
            new ShootTask(this)
        });
        
        investigateSequence = new BTSequence(new List<BTNode>()
        {
            new HeardNoiseCondition(this),
            new GainAwarenessTask(this),
            new FillAwarenessMeterTask(this),
            new IsAwareCondition(this),
            new SetAlertStateTask(this),
            new InvestigateTask(this)
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
            officerAliveSequence,
            engageEnemySequence,
            investigateSequence,
            patrolSequence
        });
    }
    private void LateUpdate()
    {
        rootAI.Evaluate();
    }
}
