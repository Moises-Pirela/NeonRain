using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeTask : BTNode
{
    private BaseAI myAI;
    
    public FleeTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        myAI.aiState = AIState.FLEEING;
        
        var fleePosition = myAI.transform.position + new Vector3(10, 0, 0);

        myAI.agent.SetDestination(fleePosition);

        myAI.myEntity.faction = Faction.NONE;

        return BTNodeStates.SUCCESS;
    }
}
