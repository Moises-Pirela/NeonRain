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

    [SerializeField] private LineRenderer grappleTarget;
    [SerializeField] private float range;
    [SerializeField] private MovementInputData _movementInputData;

    private bool isReady = false;
    private Vector3 targetVector;
    
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
        grappleTarget.SetPosition(1 , Vector3.zero);

        targetVector = Vector3.zero;
    }

    private void FindTargetLocation()
    {
        targetVector = Vector3.zero;
        
        grappleTarget.SetPosition(1 , Vector3.forward * range);
        targetVector =  fpsCam.transform.forward.normalized * range;
    }

    private void ShowReadyAnimation()
    {
        isReady = true;
        grappleTarget.gameObject.SetActive(true);
    }

    private void Grapple()
    {
        if (targetVector != Vector3.zero)
        {
            _movementInputData.Dashed = true;
            _movementInputData.DashVector = targetVector;
            _playerManager.DrainArmor(mydata.armorDrain);
            StartCoroutine(Reset());
        };
        
        ClearTarget();
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);

        _movementInputData.Dashed = false;
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
                Debug.Log("Press");
                Grapple();
                //ClearTarget();
                break;
            case HoldInteraction _:
                Debug.Log("Hold");
                if (_playerManager.Armor < mydata.armorDrain) return;
                ShowReadyAnimation();
                break;
        }
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        switch (context.interaction)
        {
            case TapInteraction _:
                //ClearTarget();
                break;
            case HoldInteraction _:
                Grapple();
                break;
        }
    }

    public override void CancelUse(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
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
