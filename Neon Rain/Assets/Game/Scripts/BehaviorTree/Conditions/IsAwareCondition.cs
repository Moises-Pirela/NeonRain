using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAwareCondition : BTNode
{
    public BaseAI myAI;
    
    public IsAwareCondition(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return myAI.AwarenessMeter >= 1 ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
