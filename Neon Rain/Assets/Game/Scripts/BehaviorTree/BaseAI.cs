﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
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

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BaseEntity))]
public abstract class BaseAI : MonoBehaviour
{
    public BTRoot rootAI;

    public UnitData unitData;

    [HideInInspector] public NavMeshAgent agent;

    private BaseEntity m_currentAttackTargetEntity;

    private Vector3 m_movePosition;

    [HideInInspector] public BaseEntity self;

    protected float attackTimer;

    public bool isAttacking;
    
    private bool hasWaypoint;
    [SerializeField] protected LayerMask attackMask;

    public bool isAttracted = false;

    protected virtual BTSequence CombatSequence()
    {
        BTSequence sequence = new BTSequence(new List<BTNode>()
        {
                new EnemySpottedCondition(this),
                InAttackRangeSelector(),
                new FaceMoveDirectionTask(this),
                new AttackTask(this)
        });

        return sequence;
    }

    protected virtual BTSequence WanderSequence()
    {
        BTSequence sequence = new BTSequence(new List<BTNode>()
        {
            HasWaypointSelector() ,
            new ReachedDestinationCondition(this),
            new SelectRandomWanderPoint(this),
            MoveSequence()
        });

        return sequence;
    }

    protected virtual BTSelector InAttackRangeSelector()
    {
        BTSelector selector = new BTSelector(new List<BTNode>()
        {
            new InAttackRangeCondition(this),
            new BTSequence(new List<BTNode>()
            {
                MoveSequence(),
                new ForceFailureTask()
            }),
        });

        return selector;
    }
    
    protected virtual BTSelector HasWaypointSelector()
    {
        BTSelector selector = new BTSelector(new List<BTNode>()
        {
            new HasWaypointCondition(this),
            new BTSequence(new List<BTNode>()
            {
                new SelectRandomWanderPoint(this),
                MoveSequence(),
            }),
            new ForceFailureTask()
        });

        return selector;
    }

    protected virtual BTSequence MoveSequence()
    {
        BTSequence sequence = new BTSequence(new List<BTNode>()
        {
            //new CanMoveCondition(this),
            //new FaceMoveDirectionTask(this),
            new MoveToTask(this)
        });

        return sequence;
    }

    protected void PopulateBtNodes(List<BTNode> btNodes)
    {
        rootAI = new BTRoot(btNodes);
    }
    
    public virtual bool CanMove()
    {
        return !isAttacking;
    }

    private void Awake()
    {
        SetComponents();
    }

    private void LateUpdate()
    {
        rootAI.Evaluate();
    }

    private void SetComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        self = GetComponent<BaseEntity>();
    }

    public bool EnemySpotted()
    {
        return m_currentAttackTargetEntity != null;
    }
    public bool HasWaypoint
    {
        get { return hasWaypoint; }

        set { hasWaypoint = value; }
    }

    public bool InAttackRange
    {
        get
        {
            if (m_currentAttackTargetEntity == null) return false;

            var distance = Vector3.Distance(CurrentAttackTarget.transform.position,transform.position) ;

            return distance <= unitData.attackRange;
        }
    }

    public Vector3 MovePosition
    {
        get { return m_movePosition; }
        set { m_movePosition = value; }
    }

    public Vector3 AttackTargetPosition
    {
        get
        {
            if (m_currentAttackTargetEntity == null) return transform.position;
            
            return m_currentAttackTargetEntity.transform.position;
        }
    }

    public BaseEntity CurrentAttackTarget
    {
        get { return m_currentAttackTargetEntity; }
        set
        {
            m_currentAttackTargetEntity = value;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isAttracted)
        {
            Invoke("ResetAgent", 1f);
        }
    }

    private void ResetAgent()
    {
        agent.enabled = true;
    }

    public abstract void Attack();

    public void Respawn()
    {
    }
}