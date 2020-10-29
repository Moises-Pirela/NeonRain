using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : BTNode
{
    private string logMessage;
    
    public DebugNode(string message)
    {
        logMessage = message;
    }
    
    public override BTNodeStates Evaluate()
    {
        Debug.Log(logMessage);
        
        return BTNodeStates.SUCCESS;
    }
}
