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
        if (Myai.agent.enabled)
            Myai.agent.SetDestination(Myai.MovePosition);
        else
        {
            return BTNodeStates.FAILURE;
        }

        return BTNodeStates.SUCCESS;
    }
}