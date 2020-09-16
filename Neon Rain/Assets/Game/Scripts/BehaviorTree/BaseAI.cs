using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    BASIC,
    OFFICER,
    LEADER
}

public enum AIState
{
    ACTIVE,
    FLEEING
}

public abstract class BaseAI : MonoBehaviour
{
    public UnitData unitData;
    
    private GameObject _currentTarget;
    public GameObject CurrentTarget
    {
        get => _currentTarget;
        set { _currentTarget = value; }
    }

    public BaseEntity myEntity;
    
    private BaseEntity _currentTargetBaseEntity;
    public BaseEntity CurrentTargetBaseEntity => _currentTargetBaseEntity;

    public NavMeshAgent agent;
    
    public BTSelector rootAI;

    [HideInInspector] public AIState aiState;

    [HideInInspector] public bool resistedFlee = false;

    [SerializeField] private GameObject _sprite;

    [SerializeField] private Camera worldCamera;

    private void Awake()
    {
        myEntity = GetComponent<BaseEntity>();
        aiState = AIState.ACTIVE;
    }


    public void SetCurrentTarget(GameObject target, BaseEntity baseEntity)
    {
        _currentTarget = target;
        _currentTargetBaseEntity = baseEntity;
    }

    public bool CheckOfficerAlive()
    {
        LayerMask layerMask = LayerMask.GetMask($"Units");
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, unitData.sightDistance * 2, hitColliders, layerMask);
        for (int i = 0; i < numColliders; i++)
        {
            var unit =  hitColliders[i].GetComponent<BaseAI>();

            if (unit.unitData.aiType == AIType.OFFICER) return true;
        }

        return false;
    }

    private void LateUpdate()
    {
          // transform.LookAt(transform.position + worldCamera.transform.rotation * Vector3.forward,
          //    worldCamera.transform.rotation * Vector3.up);
    }
}
