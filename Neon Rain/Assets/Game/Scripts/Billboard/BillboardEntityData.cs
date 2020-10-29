using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Billboard/BillboardEntity", fileName = "New BillboardEntityData")]
public class BillboardEntityData : ScriptableObject
{
    public Animator[] walkCycle;
    public Animator[] dieCycle;
    public Animator[] attackCycle;
    public Animator[] idleCycle;

    public Color[] debugAnglesCycle; 

    public bool isDynamic;
}
