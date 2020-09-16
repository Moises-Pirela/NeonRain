using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistFleeCondition : BTNode
{
    private BaseAI myAI;
    
    public ResistFleeCondition(BaseAI baseAI)
    {
        myAI = baseAI;
    }

    public override BTNodeStates Evaluate()
    {
        return myAI.resistedFlee ? BTNodeStates.FAILURE : BTNodeStates.SUCCESS;
    }
}
