using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    [SerializeField] private BaseAI myAI;
    [SerializeField] private SphereCollider myCollider;
    [SerializeField] private StateMachine StateMachine;
    [SerializeField] private LayerMask obstaclesMask;
    

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

    private void LateUpdate()
    {
        if (!myAI.EnemySpotted()) return;
        
        var transform1 = myAI.CurrentAttackTarget.transform;
        myAI.MovePosition = transform1.position + (transform1.forward * 3);
    }

    void CheckForTarget(Collider collider)
    {   
        var unit = collider.GetComponent<BaseEntity>();

        if (!unit) return;
        
        var direction = unit.transform.position - transform.position;

        if (unit.entityType == EntityType.PLAYER)
        {
            // if ((Vector3.Angle(transform.forward, direction) <= myAI.unitData.fov))
//            {
                Debug.DrawRay(transform.position, direction, Color.red, 1);
                if (!Physics.Raycast(transform.position, direction, out RaycastHit hit,
                    myAI.unitData.sightDistance, obstaclesMask))
                {
                    
                    myAI.CurrentAttackTarget = unit;
                }
            // }
            // else
            // {
            //     return;
            // }
        }
        else
        {
             if (myAI.self.faction == unit.faction) return;
             
             myAI.CurrentAttackTarget = unit;
            
            // myAI.AwarenessMeter = 1;
        }

        //StateMachine.currentState = States.ATTACKING;

        
    }
}