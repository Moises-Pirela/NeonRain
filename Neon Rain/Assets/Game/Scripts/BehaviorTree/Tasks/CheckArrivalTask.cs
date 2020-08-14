using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArrivalTask : BTNode
{
    private IBehaviorAI myAI;
    
    public CheckArrivalTask(IBehaviorAI _myAI)
    {
        myAI = _myAI;
    }

    public override BTNodeStates Evaluate()
    {
        var agentPosition = myAI.GetAgentTransform().position;
        var targetPosition = myAI.GetTargetPosition();
        
        var distance = Vector3.Distance(agentPosition, targetPosition);

        return distance < 3f ? BTNodeStates.SUCCESS : BTNodeStates.RUNNING;
    }
}
