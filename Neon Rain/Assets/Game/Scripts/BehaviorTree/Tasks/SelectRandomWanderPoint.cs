using UnityEngine;
using UnityEngine.AI;

public class SelectRandomWanderPoint : BTNode
{
    private BaseAI Myai;
    
    public SelectRandomWanderPoint(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {   
        var randomPoint = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));

        if (Myai.agent.path.status == NavMeshPathStatus.PathInvalid) return BTNodeStates.FAILURE;

        Myai.HasWaypoint = true;
        Myai.MovePosition = randomPoint;

        return BTNodeStates.SUCCESS;

    }
}