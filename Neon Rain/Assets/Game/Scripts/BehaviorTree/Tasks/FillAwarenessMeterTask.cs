using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAwarenessMeterTask : BTNode
{
    private BaseAI myAI;
    
    public FillAwarenessMeterTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        myAI.awarenessMeterFill.fillAmount = myAI.AwarenessMeter;

        return BTNodeStates.SUCCESS;
    }
}
