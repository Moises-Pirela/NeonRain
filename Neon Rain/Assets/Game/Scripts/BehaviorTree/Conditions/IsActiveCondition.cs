using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActiveCondition : BTNode
{
    private BaseAI myai;
    
    public IsActiveCondition(BaseAI baseAI)
    {
        myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return myai.aiState == AIState.ACTIVE ? BTNodeStates.FAILURE : BTNodeStates.SUCCESS;
    }
}
