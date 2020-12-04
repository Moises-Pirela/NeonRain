using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using VHS;

public class Pistol : Equipment
{
    public Camera fpsCam;
    public float damage;
    private Transform playerController;
    public GameObject impactEffect;

    public LayerMask layerMask;

    private void Awake()
    {
        mydata = SaveData.Current.inventory.pistol;
    }


    public override void SetEquipment(Camera camera, Transform player, EquipmentController equipmentController = null)
    {
        fpsCam = camera;
        playerController = player;
    }
    
    public override void Use(InputAction.CallbackContext context)
    {
        if (DebugController._instance.IsInConsole || GameMaster._current.IsPaused) return;
        
        if (PlayerEvents.Current.isAttacking) return;

        audioSource.PlayOneShot(useEffect);

        if (InputHandler.IsController())
            StartCoroutine(InputHandler.Rumble(0,0.75f,0.1f));
        
        //Gamepad.current?.SetMotorSpeeds(0f,0.75f);
        //InputSystem.ResetHaptics();

        //transform.DOPunchRotation(new Vector3(-20,0,0), 0.1f);
        
        PlayerEvents.Current.PlayerShoot();

        if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, 100, layerMask, QueryTriggerInteraction.Ignore)) return;

        Damageable target = hit.transform.GetComponent<Damageable>();
        
        // var effect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        // effect.transform.SetParent(hit.transform);
        // Destroy(effect, 2f);
        
        if (target == null) return; 
            
        target.TakeDamage(damage);
        
        // if (hit.transform.GetComponent<Explosive>())
        //         hit.transform.GetComponent<Explosive>().Explode();
        //
        // var target = hit.transform.GetComponent<EnemyManager>();
        // if (target == null) return;
        // target.Health -= damage;
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void CancelUse(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    // private bool CanShoot()
    // {
    //     //return PlayerEvents.Current.hasAmmo && !PlayerEvents.Current.isGrabbing;
    // }
    public override InputAction MyInput()
    {
        PlayerControls _playerControls = new PlayerControls();
        return _playerControls.Player.Primary_Fire;
    }

    public override bool MyReverseInput()
    {
        throw new System.NotImplementedException();
    }

    public Pistol(EquipmentData data) : base(data)
    {
        
    }
}