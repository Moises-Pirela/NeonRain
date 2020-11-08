using UnityEngine;

public class HasWaypointCondition : BTNode
{
    private BaseAI Myai;
    
    public HasWaypointCondition(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return Myai.HasWaypoint ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE; 
    }
}