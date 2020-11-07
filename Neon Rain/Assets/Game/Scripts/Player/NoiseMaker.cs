using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoiseMaker : MonoBehaviour
{
    float gizmoRadius;

    private float noiseLevel;

    [SerializeField] [Range(0, 50)] private float walkingNoise;
    [SerializeField] [Range(0, 50)] private float crouchingNoise = 0;
    [SerializeField] [Range(0, 50)] private float sprintingNoise;
    [SerializeField] [Range(0, 50)] private float shootingNoise;
    [SerializeField] [Range(0, 50)] private float meleeNoise;

    private void Start()
    {
        noiseLevel = walkingNoise;
        
        PlayerEvents.Current.onPlayerCrouch += () => { noiseLevel = crouchingNoise; };
        PlayerEvents.Current.onPlayerStand += () => { noiseLevel = walkingNoise; };

        PlayerEvents.Current.onPlayerStartSprint += () => { noiseLevel = sprintingNoise; };
        PlayerEvents.Current.onPlayerStopSprint += () => { noiseLevel = walkingNoise; };

        PlayerEvents.Current.onPlayerShoot += () => { FireNoise(shootingNoise); };

        PlayerEvents.Current.onPlayerWalk += () => { FireNoise(noiseLevel); };
    }

    public void FireNoise(float noiseLevel)
    {
        gizmoRadius = noiseLevel;
        
        LayerMask layerMask = LayerMask.GetMask($"Units");
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, noiseLevel, hitColliders, layerMask);

        for (int i = 0; i < numColliders; i++)
        {
            var position = transform.position;
            var hitPosition = hitColliders[i].transform.position;
            var direction = hitPosition - position;
            var distance = Vector3.Distance(position, hitPosition);
            var aiAgent = hitColliders[i].GetComponent<BaseAI>();
            aiAgent.SetNoiseDetected(noiseLevel);
            aiAgent.moveToPosition = transform.position - (transform.forward * -3);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 1.5f);
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}