using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachedDestinationCondition : BTNode
{
    private BaseAI myAI;
    
    public ReachedDestinationCondition(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return myAI.agent.remainingDistance < 1.0f ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
