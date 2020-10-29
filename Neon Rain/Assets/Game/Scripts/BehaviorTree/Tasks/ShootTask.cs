using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootTask : BTNode
{
    private BaseAI _myai;

    private float shootingTimer = 0;
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private LayerMask _layerMask;

    public ShootTask(BaseAI baseAI)
    {
        _myai = baseAI;
        
        _layerMask = LayerMask.GetMask("Units");
        LayerMask playerMask = LayerMask.GetMask("Player");
        _layerMask += playerMask;
    }
    
    public override BTNodeStates Evaluate()
    {
        shootingTimer += Time.deltaTime;

        if (!(shootingTimer >= _myai.unitData.fireRate)) return BTNodeStates.FAILURE;
        
        var target = _myai.CurrentTarget;
        
        var position = target.transform.position;
        
        var direction = position - _myai.transform.position;
        
        _myai.myEntity.animator.SetTrigger(Shoot);
        
        if (Physics.Raycast(_myai.transform.position + new Vector3(0,0.5f,0.6f), direction, out var hit, _myai.unitData.shootingRange, _layerMask, QueryTriggerInteraction.Collide))
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