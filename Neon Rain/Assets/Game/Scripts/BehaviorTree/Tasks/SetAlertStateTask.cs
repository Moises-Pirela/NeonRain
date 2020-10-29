using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlertStateTask : BTNode
{
    private BaseAI myAI;
    
    public SetAlertStateTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        myAI.awarenessMeterFill.color = Color.yellow;
        
        return BTNodeStates.SUCCESS;
    }
}
