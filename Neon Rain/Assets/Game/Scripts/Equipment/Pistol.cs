using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : Equipment
{
    public Camera fpsCam;
    public float damage;
    private Transform playerController;
    public GameObject impactEffect;

    public override void SetEquipment(Camera camera, Transform player, EquipmentController equipmentController = null)
    {
        fpsCam = camera;
        playerController = player;
    }

    public override void Use()
    {
        if (PlayerEvents.Current.canShoot) return;
        
        PlayerEvents.Current.canShoot = true;

        var layerMask = 1 << 8;
        var layerMask2 = layerMask + (1 << 9);

        layerMask = ~layerMask2;
        
        audioSource.PlayOneShot(useEffect);

        //transform.DOPunchRotation(new Vector3(-20,0,0), 0.1f);
        
        PlayerEvents.Current.PlayerShoot();

        if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, 100, layerMask, QueryTriggerInteraction.Ignore)) return;

        var effect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        effect.transform.SetParent(hit.transform);
        Destroy(effect, 2f);
        
        
        
        // if (hit.transform.GetComponent<Explosive>())
        //         hit.transform.GetComponent<Explosive>().Explode();
        //
        // var target = hit.transform.GetComponent<EnemyManager>();
        // if (target == null) return;
        // target.Health -= damage;
    }

    public override void LeaveUse()
    {
        throw new System.NotImplementedException();
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
}