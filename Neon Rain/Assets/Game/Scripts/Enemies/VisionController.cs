using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    [SerializeField]private BaseAI myAI;
    [SerializeField] private SphereCollider _collider;

    private void Start()
    {
        _collider.radius = myAI.unitData.sightDistance;
    }

    private void Update()
    {
        CheckForTarget();
    }

    // void OnTriggerStay(Collider hitCollider)
    // {
    //     var entity = hitCollider.GetComponent<BaseEntity>();
    //
    //     if (entity == null) return;
    //     
    //     var direction = hitCollider.transform.position - transform.position;
    //     
    //     if ((Vector3.Angle(transform.forward, direction) >= myAI.unitData.fov))
    //     {
    //         myAI.SetCurrentTarget(hitCollider.gameObject, entity);
    //     }
    //     else
    //     {
    //         myAI.SetCurrentTarget(null,null);
    //     }
    // }

    void CheckForTarget()
    {
        LayerMask layerMask = LayerMask.GetMask($"Units");
        LayerMask playerMask = LayerMask.GetMask("Player");
        layerMask += playerMask;
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, myAI.unitData.sightDistance, hitColliders, layerMask);
        for (int i = 0; i < numColliders; i++)
        {
            var unit =  hitColliders[i].GetComponent<BaseEntity>();
            
            var direction = hitColliders[i].transform.position - transform.position;

            if (unit.entityType == EntityType.PLAYER)
            {
                if ((Vector3.Angle(transform.forward, direction) >= myAI.unitData.fov))
                {
                    myAI.SetCurrentTarget(hitColliders[i].gameObject, unit);
                }
                else
                {
                    myAI.SetCurrentTarget(null,null);
                } 
            }
            else
            {
                if (myAI.myEntity.faction == unit.faction) return;

                myAI.SetCurrentTarget(hitColliders[i].gameObject, unit);
            }
            
           
        }
    }
    
}
