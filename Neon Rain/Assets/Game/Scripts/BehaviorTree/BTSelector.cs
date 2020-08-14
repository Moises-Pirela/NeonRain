using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    protected List<BTNode> myNodes = new List<BTNode>();

    public BTSelector(List<BTNode> nodes)
    {
        myNodes = nodes;
    }
    
    public override BTNodeStates Evaluate()
    {
        foreach (var child in myNodes)
        {
            switch (child.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    continue;
                case BTNodeStates.SUCCESS:
                    nodeState = BTNodeStates.SUCCESS;
                    return nodeState;
                default:
                    continue;
            }
        }

        nodeState = BTNodeStates.FAILURE;
        return nodeState;
    }
}
