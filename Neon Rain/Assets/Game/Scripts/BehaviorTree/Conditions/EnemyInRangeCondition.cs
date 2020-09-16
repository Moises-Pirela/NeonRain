using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInRangeCondition : BTNode
{
    private BaseAI _myAI;
    private Transform myTarget;
    
    public EnemyInRangeCondition( BaseAI ai)
    {
        _myAI = ai;
    }
    
    public override BTNodeStates Evaluate()
    {   
        return Vector3.Distance(_myAI.transform.position, _myAI.CurrentTarget.transform.position) <= _myAI.unitData.shootingRange 
            ? BTNodeStates.SUCCESS 
            : BTNodeStates.FAILURE;
    }
}
