using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpottedCondition : BTNode
{
    private BaseEntity _enemyBase;
    private BaseAI myAI;
    
    public EnemySpottedCondition(BaseAI myAI)
    {
        this.myAI = myAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        _enemyBase = myAI.CurrentTargetBaseEntity;
        
        if (_enemyBase == null) return BTNodeStates.FAILURE;
        
        return _enemyBase.faction != myAI.myEntity.faction ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
