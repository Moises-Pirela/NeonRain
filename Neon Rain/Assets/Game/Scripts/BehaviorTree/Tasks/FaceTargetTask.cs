using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FaceTargetTask : BTNode
{
    private BaseAI myAI;

    public FaceTargetTask(BaseAI baseAI)
    {
        myAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        myAI.transform.DOKill();
        
        myAI.transform.DOLookAt(myAI.CurrentTarget.transform.position,0.1f);

        return BTNodeStates.SUCCESS;
    }
}
