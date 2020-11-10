using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Explosives/Explosive", fileName = "New Explosive")]
public class ExplosiveData : ScriptableObject
{
    public float explosionRadius;
    public float explosionForce;
    public float explosionDamage;
}
