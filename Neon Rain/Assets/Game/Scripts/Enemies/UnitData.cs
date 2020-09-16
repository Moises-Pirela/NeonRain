using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ranks
{
    STURMMANN,
    STURMSCHARFÜHRER,
    HAUPTSTURMFÜHRER
}

[CreateAssetMenu(fileName = "New_Unit_Data",menuName = "Units/Data")]
public class UnitData : ScriptableObject
{
    public Ranks unitRank;
    
    public AIType aiType;
    
    public float shootingRange;
    public float sightDistance;
    public float fov;
    public float fireRate;
    public float damage;
}


