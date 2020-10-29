using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPatrolCondition : BTNode
{
    private BaseAI myAI;
    private IPatrol myPatrol;
    
    public IsPatrolCondition(BaseAI ai)
    {
        myAI = ai;

        myPatrol = ai.GetComponent<IPatrol>();
    }
    
    public override BTNodeStates Evaluate()
    {
        return myAI.aiBehavior == AIBehavior.PATROL ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
