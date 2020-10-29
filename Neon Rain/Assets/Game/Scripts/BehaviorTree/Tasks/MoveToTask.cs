using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTask : BTNode
{
    private BaseAI _myagent;
    
    public MoveToTask(BaseAI agent)
    {
        _myagent = agent;
    }
    
    public override BTNodeStates Evaluate()
    {
        var destination = _myagent.CurrentTarget.transform.position + new Vector3(Random.Range(0, 15), 0, 0);
        
        _myagent.agent.SetDestination(destination);

        return BTNodeStates.SUCCESS;
    }
}
