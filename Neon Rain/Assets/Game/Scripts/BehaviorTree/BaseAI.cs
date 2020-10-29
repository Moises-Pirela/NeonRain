using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

public enum AIBehavior
{
    IDLE,
    PATROL
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

    private Vector3 investigationTarget;

    public Vector3 InvestigationTarget
    {
        get => investigationTarget;
        set => investigationTarget = value;
    }
    
    public BaseEntity myEntity;

    private BaseEntity _currentTargetBaseEntity;
    public BaseEntity CurrentTargetBaseEntity => _currentTargetBaseEntity;

    public NavMeshAgent agent;

    private float awarenessMeter = 0;

    public float AwarenessMeter
    {
        get => awarenessMeter;
        set => awarenessMeter = Mathf.Clamp(value,0f,1f);
    }

    public const float awarenessMeterThreshold = 1f;

    public BTSelector rootAI;

    [HideInInspector] public AIState aiState;
    public AIBehavior aiBehavior;

    [HideInInspector] public bool resistedFlee = false;

    [HideInInspector] public bool heardNoise = false;

    [HideInInspector] public bool playerSpotted = false;
    [HideInInspector] public float noiseDetected = 0;

    [SerializeField] private GameObject _sprite;

    [SerializeField] private Camera worldCamera;

    public Image awarenessMeterFill;

    private void Awake()
    {
        myEntity = GetComponent<BaseEntity>();
        aiState = AIState.ACTIVE;
    }

    public void Respawn()
    {
        AwarenessMeter = 0;
        CurrentTarget = null;
        playerSpotted = false;
        noiseDetected = 0;
        heardNoise = false;
        resistedFlee = false;
        _currentTargetBaseEntity = null;
        investigationTarget = Vector3.zero;
        agent.isStopped = true;
        aiState = AIState.ACTIVE;
    }


    public void SetCurrentTarget(GameObject target, BaseEntity baseEntity = null)
    {
        _currentTarget = target;
        _currentTargetBaseEntity = baseEntity;
    }

    public bool CheckOfficerAlive()
    {
        LayerMask layerMask = LayerMask.GetMask($"Units");
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders =
            Physics.OverlapSphereNonAlloc(transform.position, unitData.sightDistance * 2, hitColliders, layerMask);
        for (int i = 0; i < numColliders; i++)
        {
            var unit = hitColliders[i].GetComponent<BaseAI>();

            if (unit.unitData.aiType == AIType.OFFICER) return true;
        }

        return false;
    }

    public void SetNoiseDetected(float val)
    {
        noiseDetected = val;
        heardNoise = true;
    }

    public void ResetNoiseDetection()
    {
        heardNoise = false;
        noiseDetected = 0;
    }

    public void ResetAwareness()
    {
        AwarenessMeter = 0;
        playerSpotted = false;
        SetCurrentTarget(null, null);
    }
}