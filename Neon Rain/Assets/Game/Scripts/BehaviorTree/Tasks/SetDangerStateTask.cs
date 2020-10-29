using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDangerStateTask : BTNode
{
    private BaseAI myAI;
    
    public SetDangerStateTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        myAI.awarenessMeterFill.color = Color.red;
        
        return BTNodeStates.SUCCESS;
    }
}
