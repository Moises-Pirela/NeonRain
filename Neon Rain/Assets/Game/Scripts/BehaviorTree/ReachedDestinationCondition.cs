using UnityEngine;

public class ReachedDestinationCondition : BTNode
{
    private BaseAI MyAI;

    public ReachedDestinationCondition(BaseAI baseAI)
    {
        MyAI = baseAI;
    }

    public override BTNodeStates Evaluate()
    {
        var distance = Vector3.Distance(MyAI.transform.position, MyAI.MovePosition);

        var reachedDestination = distance <= 1f;

        return reachedDestination ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}