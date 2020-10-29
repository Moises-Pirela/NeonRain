using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dagger : Equipment
{
    private Camera fpsCam;
    private bool isAttacking = false;
    public float hitRange;
    public float damage;
    public AudioClip hitEffect;

    private void Awake()
    {
        mydata = SaveData.Current.inventory.dagger;
    }
    
    public override void SetEquipment(Camera camera, Transform player, EquipmentController equipmentController)
    {
        fpsCam = camera;
    }

    public override void Use(InputAction.CallbackContext context)
    {
        if (DebugController._instance.IsInConsole || GameMaster._current.IsPaused) return;
        
        if (PlayerEvents.Current.isAttacking) return;
        
        var layerMask = 1 << 8;
        var layerMask2 = layerMask + (1 << 9);

        layerMask = ~layerMask2;
        
       // audioSource.PlayOneShot(useEffect);
       
       PlayerEvents.Current.PlayerMelee();

       if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, hitRange, layerMask, QueryTriggerInteraction.Ignore)) return;
        
        BaseEntity target = hit.transform.GetComponent<BaseEntity>();
        
        if (target == null) return; 
            
        target.TakeDamage(damage);
        
        //audioSource.PlayOneShot(hitEffect);
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public override InputAction MyInput()
    {
        PlayerControls _playerControls = new PlayerControls();
        return _playerControls.Player.Primary_Fire;
    }

    public override bool MyReverseInput()
    {
        throw new System.NotImplementedException();
    }

    public Dagger(EquipmentData data) : base(data)
    {
    }
}
