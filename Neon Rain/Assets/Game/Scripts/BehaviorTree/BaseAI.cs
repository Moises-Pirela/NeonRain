using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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
    public BTSequence rootAI;

    public UnitData unitData;

    [HideInInspector] public NavMeshAgent agent;

    private BaseEntity m_currentAttackTargetEntity;

    private Vector3 m_movePosition;

    [HideInInspector] public BaseEntity self;

    protected float attackTimer;

    public bool isAttacking;
    
    [SerializeField] protected LayerMask attackMask;

    protected virtual BTSequence Seq_CombatSequence()
    {
        BTSequence sequence = new BTSequence(new List<BTNode>()
        {
                new EnemySpottedCondition(this),
                new DebugNode("ENEMY SPOTTED"),
                Sel_InAttackRangeSelector(),
                new DebugNode("IN ATTACK RANGE"),
                new FaceMoveDirectionTask(this),
                new AttackTask(this)
        });

        return sequence;
    }

    protected virtual BTSelector Sel_InAttackRangeSelector()
    {
        BTSelector selector = new BTSelector(new List<BTNode>()
        {
            new InAttackRangeCondition(this),
            Seq_MoveSequence(),
        });

        return selector;
    }

    protected virtual BTSequence Seq_MoveSequence()
    {
        BTSequence sequence = new BTSequence(new List<BTNode>()
        {
            //new CanMoveCondition(this),
            new FaceMoveDirectionTask(this),
            new MoveToTask(this)
        });

        return sequence;
    }

    protected void PopulateBtNodes(List<BTNode> btNodes)
    {
        rootAI = new BTSequence(btNodes);
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

    public bool InAttackRange
    {
        get
        {
            if (m_currentAttackTargetEntity == null) return false;
            
            return Vector3.Distance(transform.position, m_currentAttackTargetEntity.transform.position) <= unitData.attackRange;
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

    public abstract void Attack();

    public void Respawn()
    {
    }
}