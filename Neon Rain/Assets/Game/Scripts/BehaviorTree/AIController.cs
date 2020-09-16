using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    GUARD,
    PATROL
}

public class AIController : MonoBehaviour, IBehaviorAI
{
    public float sightDistance;
    public float fieldOfView;

    private float awarenessMeter;
    private float noiseDetected; 
    
    private Vector3 targetPosition;
    [HideInInspector] public Vector3 playerPosition;
    private NavMeshAgent navMeshAgent;
    public EnemyType enemyType;
    
    public BTSelector rootAI;
    public BTSelector TypeSelector;
    private BTSequence MoveSequence;
    private BTSequence CheckArrivalSequence;
    private BTSequence GuardSequence;

    public bool playerInRange = false;
    public bool isLocked = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {   
        TypeSelector = new BTSelector(new List<BTNode>()
        {
            new CheckTypeTask(this)
        });
        
        GuardSequence = new BTSequence(new List<BTNode>()
        {
            new PlayerSpottedTask(this),
            new GenerateAwarenessMeterTask(this),
            new MoveTask(this, 20f, navMeshAgent)
        });
        
        CheckArrivalSequence = new BTSequence(new List<BTNode>()
        {
            new CheckArrivalTask(this),
            new SearchTask(this, 50f)
        });
        
        MoveSequence = new BTSequence(new List<BTNode>()
        {
            new MoveTask(this, 20f, navMeshAgent)
        });
        
        rootAI = new BTSelector(new List<BTNode>()
        {
           GuardSequence
        });

        //new SearchTask(this, 50f);
    }

    private void Update()
    {
        rootAI.Evaluate();
    }

    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }

    public Vector3 SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        return targetPosition;
    }

    public Transform GetAgentTransform()
    {
        return gameObject.transform;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public Vector3 GetEnemyPosition()
    {
        throw new NotImplementedException();
    }

    public GameObject SetCurrentTarget(GameObject target)
    {
        throw new NotImplementedException();
    }

    public bool PlayerInRange()
    {
        return playerInRange; //Vector3.Distance(GetAgentTransform().position, GetPlayerPosition()) <= sightDistance;
    }

    public bool PlayerSpotted()
    {
        var direction = playerPosition - transform.position;

        if (!(Vector3.Angle(transform.forward, direction) <= fieldOfView)) return false;

        var layerMask = 1 << 2;
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, direction, out var hit, sightDistance, layerMask))
        {
            Debug.Log(hit.collider.name);
            
            if (!hit.collider.CompareTag("Player")) return false;
            
            isLocked = true;
            return true;
        }

        return false;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public float GetAwarenessMeter()
    {
        return awarenessMeter;
    }

    public float SetAwarenessMeter(float tick)
    {
        awarenessMeter += tick;
        awarenessMeter =  Mathf.Clamp(awarenessMeter,0,1);
        return awarenessMeter;
    }

    public float GetDetectedNoise()
    {
        return noiseDetected;
    }

    public float SetDetectedNoise(float noise)
    {
        noiseDetected =  noise;
        return noiseDetected;
    }
}
