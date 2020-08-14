using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTNode
{
    private List<BTNode> myNodes = new List<BTNode>();
    
    public BTSequence(List<BTNode> nodes)
    {
        myNodes = nodes;
    }

    public override BTNodeStates Evaluate()
    {
        var childRunning = false;

        foreach (var child in myNodes)
        {
            switch (child.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    nodeState = BTNodeStates.FAILURE;
                    return nodeState;
                case BTNodeStates.SUCCESS:
                    continue;
                case BTNodeStates.RUNNING:
                    childRunning = true;
                    continue;
                default:
                    nodeState = BTNodeStates.SUCCESS;
                    return nodeState;
            }
        }

        nodeState = childRunning ? BTNodeStates.RUNNING : BTNodeStates.SUCCESS;
        return nodeState;
    }
}
