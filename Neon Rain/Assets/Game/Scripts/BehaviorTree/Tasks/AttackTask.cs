using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTask : BTNode
{
    private BaseAI _myai;

    private float shootingTimer = 0;
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private LayerMask _layerMask;

    public AttackTask(BaseAI baseAI)
    {
        _myai = baseAI;
        
        _layerMask = LayerMask.GetMask("Units");
        LayerMask playerMask = LayerMask.GetMask("Player");
        _layerMask += playerMask;
    }
    
    public override BTNodeStates Evaluate()
    {
        _myai.Attack();
        
        return BTNodeStates.SUCCESS;
    }
}