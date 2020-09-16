using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NaziTrooperAIController : BaseAI
{
    private BTSequence engageEnemySequence;
    private BTSelector targetInRangeSelector;
    private BTSequence officerAliveSequence;
    
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
            targetInRangeSelector,
            new ShootTask(this)
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
            engageEnemySequence
        });
    }
    private void FixedUpdate()
    {
        rootAI.Evaluate();
    }
    
}
