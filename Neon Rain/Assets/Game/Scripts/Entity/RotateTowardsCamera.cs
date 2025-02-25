﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour {

    [SerializeField] private Camera worldCamera;

    public GameObject _sprite;

    private void Start()
    {
    }

    private void LateUpdate() {
        Vector3 cameraPosition = worldCamera.transform.position;
        cameraPosition.y = 0f;
        Vector3 position = -transform.position;
        position.y = 0f;
        
        Vector3 dirToCamera = (cameraPosition - position).normalized;
        float angleToCamera = GetAngleFromVectorFloat(dirToCamera);
        transform.localEulerAngles = new Vector3(0f, -angleToCamera + 90 + 180, 0f);
        // var targetVector = transform.position - worldCamera.transform.position;
        // transform.rotation = Quaternion.LookRotation(targetVector, worldCamera.transform.rotation * Vector3.up);
    }
    
    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;

    }

}
