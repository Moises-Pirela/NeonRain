using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerDeadCondition : BTNode
{
    private BaseAI myai;
    
    public OfficerDeadCondition(BaseAI baseAI)
    {
        myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        bool officerAlive = myai.CheckOfficerAlive();
        
        return myai.CheckOfficerAlive() ?  BTNodeStates.FAILURE : BTNodeStates.SUCCESS;
    }
}
