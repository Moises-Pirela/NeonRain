using UnityEngine;

public class GenerateAwarenessMeterTask : BTNode
{
    private IBehaviorAI myAI;

    public GenerateAwarenessMeterTask(IBehaviorAI ai)
    {
        myAI = ai;
    }
    
    public override BTNodeStates Evaluate()
    {
        var canSeePlayer = myAI.PlayerSpotted();
        var tickBase = canSeePlayer ? 1f : 0.01f;
        
        var noiseLevelDetected = Mathf.Clamp(myAI.GetDetectedNoise(), 1, myAI.GetDetectedNoise());
        var distance = Vector3.Distance(myAI.GetPlayerPosition(), myAI.GetAgentTransform().position);

        var realTick = !(myAI.GetDetectedNoise() <= 0 && !canSeePlayer) ?  
            tickBase + (noiseLevelDetected / distance) :   //(tickBase * noiseLevelDetected) / (distance * tickBase)
            -0.2f;

        myAI.SetAwarenessMeter(realTick * Time.deltaTime);

        return myAI.GetAwarenessMeter() >= 1 ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}