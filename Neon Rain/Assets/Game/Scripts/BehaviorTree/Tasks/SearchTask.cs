using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTask : BTNode
{
    private float range;
    private IBehaviorAI myAI;

    public SearchTask(IBehaviorAI _myAI, float _range)
    {
        myAI = _myAI;
        range = _range;
    }

    public override BTNodeStates Evaluate()
    {
        myAI.SetTargetPosition(Random.insideUnitSphere * range);

        return BTNodeStates.SUCCESS;
    }
}
