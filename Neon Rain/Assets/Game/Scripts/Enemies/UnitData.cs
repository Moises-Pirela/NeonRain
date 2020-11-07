using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Ranks
{
    STORMTROOPER,
    OFFICER,
    HAMMER_HEAD
}

[CreateAssetMenu(fileName = "New_Unit_Data",menuName = "Units/Data")]
public class UnitData : ScriptableObject
{
    public Ranks unitRank;
    
    public AIType aiType;
    
    [FormerlySerializedAs("shootingRange")] public float attackRange;
    public float sightDistance;
    public float fov;
    public float fireRate;
    public float damage;
}


