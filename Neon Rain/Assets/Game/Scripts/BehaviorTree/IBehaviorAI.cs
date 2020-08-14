using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IBehaviorAI
{
    Vector3 GetPlayerPosition();
    Vector3 SetTargetPosition(Vector3 targetPosition);
    Transform GetAgentTransform();
    Vector3 GetTargetPosition();
    NavMeshAgent GetNavMeshAgent();
    EnemyType GetEnemyType();
    bool PlayerInRange();
    bool PlayerSpotted();
    bool IsLocked();
    float GetAwarenessMeter();
    float SetAwarenessMeter(float tick);
    float GetDetectedNoise();
    float SetDetectedNoise(float noise);
}
