using UnityEngine;

public class PlayerSpottedTask : BTNode
{
    private IBehaviorAI myAI;

    public PlayerSpottedTask(IBehaviorAI ai)
    {
        myAI = ai;
    }
    
    public override BTNodeStates Evaluate()
    {
        //if (!myAI.PlayerInRange() && !myAI.IsLocked()) return BTNodeStates.FAILURE;
        
        var target = myAI.GetPlayerPosition();
        myAI.SetTargetPosition(target);

        return BTNodeStates.SUCCESS;
    }
}