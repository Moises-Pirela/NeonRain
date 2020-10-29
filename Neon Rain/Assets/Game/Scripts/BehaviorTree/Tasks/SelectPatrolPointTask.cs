using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPatrolPointTask : BTNode
{
    private IPatrol myAI;
    
    public SelectPatrolPointTask(IPatrol ai)
    {
        myAI = ai;
    }
    
    public override BTNodeStates Evaluate()
    {
        var randomWaypoint = myAI._patrolPoints[Random.Range(0, myAI._patrolPoints.Count)];

        myAI._currentWaypoint = randomWaypoint;
        
        Debug.LogFormat("Selected");
        
        return BTNodeStates.SUCCESS;
    }
}
