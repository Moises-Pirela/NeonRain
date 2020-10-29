using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTask : BTNode
{
    private BaseAI _baseAI;
    private IPatrol _patrol;

    public PatrolTask(BaseAI baseAI, IPatrol iPatrol)
    {
        _baseAI = baseAI;
        _patrol = iPatrol;
    }

    public override BTNodeStates Evaluate()
    {
        _baseAI.agent.SetDestination(_patrol._currentWaypoint.transform.position);
        
        return BTNodeStates.SUCCESS;
    }
}
