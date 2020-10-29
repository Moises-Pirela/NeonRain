using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainAwarenessTask : BTNode
{
    private BaseAI _myAI;
    
    public GainAwarenessTask(BaseAI baseAI)
    {
        _myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        if (_myAI.playerSpotted || _myAI.heardNoise)
        {
            
            var tickbase = _myAI.playerSpotted ? 0.35f : 0.5f;

            var position = _myAI.transform.position;
            
            var distanceToPlayer =
                Vector3.Distance(AIBlackboard._current.GetPlayerPosition(), position);

            var distanceToNoise = Vector3.Distance(_myAI.InvestigationTarget, position);

            var realTick = _myAI.playerSpotted
                ? tickbase / (AIBlackboard._current.GetPlayerExposure() * distanceToPlayer)
                : tickbase * (_myAI.noiseDetected/distanceToNoise);

            _myAI.AwarenessMeter += realTick;
            
            _myAI.ResetNoiseDetection();
        }
        
         
        
        return BTNodeStates.SUCCESS;
    }
}
