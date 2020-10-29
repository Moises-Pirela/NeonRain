using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using VHS;

public class GravityGrapple : Equipment
{
    private Camera fpsCam;
    private Transform player;

    [SerializeField] private ParticleSystem grappleTarget;
    [SerializeField] private float range;
    [SerializeField] private MovementInputData _movementInputData;

    private bool isReady = false;
    private Vector3 targetLocation;
    
    private void Awake()
    {
        mydata = SaveData.Current.inventory.gravityGrapple;
    }

    private void FixedUpdate()
    {
        if (isReady)
        {
            FindTargetLocation();
        }
    }

    private void ClearTarget()
    {
        isReady = false;
        
        grappleTarget.gameObject.SetActive(false);

        targetLocation = Vector3.zero;
    }

    private void FindTargetLocation()
    {
        targetLocation = Vector3.zero;
        
        if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, range)) return;

        grappleTarget.transform.position = hit.point;
        targetLocation = hit.point;

    }

    private void ShowReadyAnimation()
    {
        transform.DOPunchPosition(Vector3.back, 0.2f).onComplete += () =>
        {
            isReady = true;
            grappleTarget.gameObject.SetActive(true);
            grappleTarget.Play();
        };
    }

    private void Grapple()
    {
        if (targetLocation != Vector3.zero)
        {
            player.transform.DOMove(targetLocation, 0.5f);
            _playerManager.DrainArmor(mydata.armorDrain);
        };
        
        ClearTarget();
    }

    public override void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null)
    {
        fpsCam = camera;
        this.player = player;
    }

    public override void Use(InputAction.CallbackContext context)
    {
        switch (context.interaction)
        {
            case TapInteraction _:
                ClearTarget();
                break;
            case HoldInteraction _:
                ShowReadyAnimation();
                break;
        }
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        switch (context.interaction)
        {
            case TapInteraction _:
                ClearTarget();
                break;
            case HoldInteraction _:
                Grapple();
                break;
        }
    }

    public override InputAction MyInput()
    {
        throw new System.NotImplementedException();
    }

    public override bool MyReverseInput()
    {
        throw new System.NotImplementedException();
    }

    public GravityGrapple(EquipmentData data) : base(data)
    {
    }
}
