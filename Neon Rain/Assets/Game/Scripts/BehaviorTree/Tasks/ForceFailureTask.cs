using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFailureTask : BTNode
{
    public override BTNodeStates Evaluate()
    {
        return BTNodeStates.FAILURE;
    }
}
