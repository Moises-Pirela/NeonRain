using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFailCondition : BTNode
{
    public override BTNodeStates Evaluate()
    {
        return BTNodeStates.FAILURE;
    }
}
