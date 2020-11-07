using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MoveToTask : BTNode
{
    private BaseAI Myai;
    
    public MoveToTask(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    public override BTNodeStates Evaluate()
    {
        Myai.agent.SetDestination(Myai.MovePosition);

        Myai.agent.isStopped = !Myai.CanMove();
        
        return BTNodeStates.SUCCESS;
    }
}