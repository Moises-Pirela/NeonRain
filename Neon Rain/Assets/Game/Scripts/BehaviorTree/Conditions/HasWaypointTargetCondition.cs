using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasWaypointTargetCondition : BTNode
{
    private IPatrol _patrol;
    
    public HasWaypointTargetCondition(IPatrol iPatrol)
    {
        _patrol = iPatrol;
    }
    
    public override BTNodeStates Evaluate()
    {
        return _patrol._currentWaypoint != null ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
