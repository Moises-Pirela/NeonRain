using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeardNoiseCondition : BTNode
{
    private BaseAI myAI;
    
    public HeardNoiseCondition(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return myAI.heardNoise ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
