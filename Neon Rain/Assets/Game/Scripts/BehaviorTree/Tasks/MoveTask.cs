using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTask : BTNode
{
    private IBehaviorAI myAi;
    //private Transform agentPosition;
    //private Vector3 targetPosition;
    private float range;
    private NavMeshAgent agent;

    public MoveTask(IBehaviorAI _myAi, float _range, NavMeshAgent _agent)
    {
        myAi = _myAi;
        range = _range;
        agent = _agent;
    }
    
    public override BTNodeStates Evaluate()
    {
        var agentPosition = myAi.GetAgentTransform().position;
        var targetPosition = myAi.GetTargetPosition();

        agent.SetDestination(targetPosition);

        return BTNodeStates.SUCCESS;

    }
}
