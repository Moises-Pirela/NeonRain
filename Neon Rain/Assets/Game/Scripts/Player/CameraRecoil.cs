using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Recoil Settings:")]
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;

    [Header("Hipfire Recoil:")]
    public Vector3 recoilRotation = new Vector3(2f,2f,2f);

    [Header("Aiming Recoil:")]
    public Vector3 recoilRotationAiming = new Vector3(0.5f, 0.5f, 1.5f);
    
    [Header("Camera Shake:")]
    public Vector3 recoilCameraShake = new Vector3(0.5f, 0.5f, 1.5f);
    

    [Header("State:")]
    public bool aiming;

    private Vector3 currentRotation;
    
    private Vector3 Rot;

    void Start()
    {
        PlayerEvents.Current.onPlayerShoot += Shoot;
        PlayerEvents.Current.onShake += Shake;
    }

    void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
    }

    
    public void Shake()
    {
        var yRotation = Random.Range(-recoilCameraShake.y, recoilCameraShake.y) * 15;
        var zRotation = Random.Range(-recoilCameraShake.z, recoilCameraShake.z) * 15;
        currentRotation += new Vector3(-recoilCameraShake.x, yRotation, zRotation);
    }

    public void Shoot()
    {
        if (aiming)
        {
            var yRotation = Random.Range(-recoilRotationAiming.y, recoilRotationAiming.y);
            var zRotation = Random.Range(-recoilRotationAiming.z, recoilRotationAiming.z);
            currentRotation += new Vector3(-recoilRotationAiming.x, yRotation, zRotation);
        }
        else
        {
            var yRotation = Random.Range(-recoilRotation.y, recoilRotation.y);
            var zRotation = Random.Range(-recoilRotation.z, recoilRotation.z);
            currentRotation += new Vector3(-recoilRotation.x, yRotation, zRotation);
        }
    }
}
