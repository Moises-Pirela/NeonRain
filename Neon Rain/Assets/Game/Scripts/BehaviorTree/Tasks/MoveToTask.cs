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
        var position = (_myagent.CurrentTarget.transform.position + new Vector3(Random.Range(0, 5),0,0)) ;
        
        _myagent.agent.SetDestination(position);

        return BTNodeStates.SUCCESS;
    }
}
