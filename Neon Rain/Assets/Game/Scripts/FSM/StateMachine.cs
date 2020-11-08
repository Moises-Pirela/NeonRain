using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum States
{
    IDLE,
    ATTACKING,
    CHASING,
    WANDER,
    PATROL
}

public abstract class StateMachine : MonoBehaviour
{
    public States currentState;
    
    public UnitData unitData;

    [HideInInspector] public NavMeshAgent agent;
    
    [HideInInspector] public BaseEntity self;
    
    [SerializeField] protected LayerMask attackMask;

    protected float attackTimer;

    public bool isAttacking;

    private BaseEntity _currentTarget;
    public BaseEntity CurrentTarget
    {
        get { return _currentTarget; }
        set
        {
            _currentTarget = value;
        }
    }


    public virtual void IdleAction()
    {
    }
    
    public virtual void AttackAction()
    {
    }
    
    public virtual void ChaseAction()
    {
    }
    
    public virtual void WanderPatrolAction()
    {
    }

    public void Evaluate()
    {
        switch (currentState)
        {
            case States.IDLE:
                IdleAction();
                break;
            case States.ATTACKING:
                AttackAction();
                break;
            case States.CHASING:
                ChaseAction();
                break;
            case States.WANDER:
                WanderPatrolAction();
                break;
            case States.PATROL:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SetComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        self = GetComponent<BaseEntity>();
    }

    private void Awake()
    {
        SetComponents();
    }

    private void Update()
    {
        Evaluate();
    }
}