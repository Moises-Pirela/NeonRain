using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateTask : BTNode
{
    private BaseAI myAI;
    
    public InvestigateTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        var target = myAI.InvestigationTarget;
        
        myAI.agent.SetDestination(target);

        return BTNodeStates.SUCCESS;
    }
}
