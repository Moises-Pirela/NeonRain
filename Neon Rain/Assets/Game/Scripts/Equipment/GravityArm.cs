using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class GravityArm : Equipment
{
    private Camera fpsCam;
    private Transform player;
    private EquipmentController equipmentController;

    public float range;
    public override void SetEquipment(Camera camera, Transform player, EquipmentController equipmentController)
    {
        fpsCam = camera;
        this.player = player;
        this.equipmentController = equipmentController;

        PlayerEvents.Current.onPlayerGrappleReachLocation += OnGrappleReachLocation;

        PlayerEvents.Current.onPlayerGrappleEnemyDone += OnEnemyGrappleDone;

        PlayerEvents.Current.onKill += OnKillEnemy;
    }

    private void Update()
    {
        if (PlayerEvents.Current.isGrappling)
        {
            player.position =
                Vector3.MoveTowards(player.position, equipmentController.targetLocation, 25 * Time.deltaTime);
        }
    }

    public override void Use()
    {
        if (!PlayerEvents.Current.isGrappling)
        {
            var layerMask = 1 << 2;

            layerMask = ~layerMask;
            
            var ray = fpsCam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, range, layerMask, QueryTriggerInteraction.Ignore)) return;
            
            equipmentController.targetLocation = hit.point;
            PlayerEvents.Current.isGrappling = true;

            //PlayerEvents.Current.PlayerShootGrapple();
            //audioSource.Play();;
        }
        else
        {
            PlayerEvents.Current.isGrappling = false;
            PlayerEvents.Current.isPulling = false;
        }
    }

    public override void LeaveUse()
    {
        throw new System.NotImplementedException();
    }

    public override InputAction MyInput()
    {
        PlayerControls _playerControls = new PlayerControls();
        return _playerControls.Player.Secondary_Fire;
    }

    public override bool MyReverseInput()
    {
        return false;
    }

    void OnGrappleReachLocation()
    {
        PlayerEvents.Current.isGrappling = false;

        //playerAnim.SetBool("Grapple", isGrappling);
    }
    void OnEnemyGrappleDone()
    {
        OnGrappleReachLocation();
        PlayerEvents.Current.isPulling = false;
    }
    void OnKillEnemy()
    {
        PlayerEvents.Current.isPulling = false;
        PlayerEvents.Current.isGrappling = false;
    }
}