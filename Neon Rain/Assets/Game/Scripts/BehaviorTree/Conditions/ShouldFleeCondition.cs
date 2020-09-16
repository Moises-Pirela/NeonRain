using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldFleeCondition : BTNode
{
    private BaseAI myAI;

    public ShouldFleeCondition(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        if (myAI.aiState == AIState.FLEEING) return BTNodeStates.FAILURE;

        float fleeChance = Random.Range(0f, 1f);
        
        if (fleeChance <= 0.3f) return BTNodeStates.SUCCESS;
        
        myAI.resistedFlee = true;
        
        return BTNodeStates.FAILURE;
    }
}
