using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    [SerializeField] private BaseAI myAI;
    [SerializeField] private SphereCollider myCollider;

    // IEnumerator SurveyArea()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(0.2f);
    //         CheckForTarget();
    //     }
    // }
    //
    // private void Start()
    // {
    //     StartCoroutine(SurveyArea());
    // }

    private void Awake()
    {
        myCollider.radius = myAI.unitData.sightDistance;
    }

    private void OnTriggerStay(Collider other)
    {
        CheckForTarget(other);
    }

    private void Update()
    {
        //CheckForTarget();
    }

    void CheckForTarget(Collider collider)
    {   
        LayerMask layerMask = LayerMask.GetMask("Units");
        LayerMask playerMask = LayerMask.GetMask("Player");
        layerMask += playerMask;
        LayerMask obstaclesMask = ~layerMask;


        var unit = collider.GetComponent<BaseEntity>();

        if (!unit) return;
        
        var direction = unit.transform.position - transform.position;

        if (unit.entityType == EntityType.PLAYER)
        {
            if ((Vector3.Angle(transform.forward, direction) <= myAI.unitData.fov))
            {
                if (Physics.Raycast(transform.forward, direction, out RaycastHit hit,
                    myAI.unitData.sightDistance, obstaclesMask))
                {
                    return;
                }
            }
        }
        else
        {
             if (myAI.self.faction == unit.faction) return;
            
            // myAI.AwarenessMeter = 1;
        }

        myAI.CurrentAttackTarget = unit;
        myAI.MovePosition = unit.transform.position;
    }
}