using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootTask : BTNode
{
    private BaseAI _myai;

    private float shootingTimer = 0;
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    public ShootTask(BaseAI baseAI)
    {
        _myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        var target = _myai.CurrentTarget;
        
        shootingTimer += Time.deltaTime;

        var position = target.transform.position;
        var direction = position - _myai.transform.position;
        
        var layerMask = 1 << 2;

        layerMask = ~layerMask;

        if (!(shootingTimer >= _myai.unitData.fireRate)) return BTNodeStates.FAILURE;

        _myai.myEntity.animator.SetTrigger(Shoot);

        if (Physics.Raycast(_myai.transform.position, direction, out var hit, _myai.unitData.shootingRange, layerMask))
        {   
            var targetEntity = hit.transform.GetComponent<BaseEntity>();

            if (targetEntity != null)
            {
                targetEntity.TakeDamage(_myai.unitData.damage);
                
            }
        }

        //audioSource.PlayOneShot(audioSource.clip);

        shootingTimer = 0;
        
        return BTNodeStates.SUCCESS;
    }
}