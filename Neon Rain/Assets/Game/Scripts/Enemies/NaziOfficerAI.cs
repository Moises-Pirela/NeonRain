using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaziOfficerAI : BaseAI
{
    public GameObject grenadePrefab;

    public Transform grenadeSpawnPosition;
    
    void Start()
    {
        attackTimer = unitData.fireRate - 1;
        
        rootAI = new BTRoot(new List<BTNode>()
        {
            CombatSequence(),
        });
    }
    
    public override void Attack()
    {
        attackTimer += Time.deltaTime;
        
        if (!(attackTimer >= unitData.fireRate)) return;
        
        //if (isAttacking) return;

        //StartCoroutine(AttackSequence());
        
        var grenade =  Instantiate(grenadePrefab, grenadeSpawnPosition.position, Quaternion.identity).GetComponent<Grenade>();
        var direction = CurrentAttackTarget.transform.position - transform.position;
        
        grenade.Launch(direction);
        
        attackTimer = 0;
        
    }
    
}
