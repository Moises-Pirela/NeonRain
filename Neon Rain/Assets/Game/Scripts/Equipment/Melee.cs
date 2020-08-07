using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : Equipment
{
    private Camera fpsCam;
    private bool isAttacking = false;
    public float hitRange;
    public float damage;
    public AudioClip hitEffect;
    
    public override void SetEquipment(Camera camera, Transform player, EquipmentController equipmentController)
    {
        fpsCam = camera;
    }

    public override void Use()
    {
        if (isAttacking) return;
        
        var layerMask = 1 << 11;
        var layerMask2 = layerMask + (1 << 2);

        layerMask = ~layerMask2;
        
        audioSource.PlayOneShot(useEffect);
        isAttacking = true;
        
        transform.DOLocalMoveX(0.1f, 0.2f).OnComplete(() =>
        {

            transform.DOLocalMoveX(1f,0.5f);

        });
        
        transform.DOLocalRotate(new Vector3(52, -90, 0), 0.2f).OnComplete(() =>
        {

            transform.DOLocalRotate(new Vector3(0, -90, 78.4f),0.5f).OnComplete(() => { isAttacking = false; });

        });

        var ray = fpsCam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, hitRange, layerMask, QueryTriggerInteraction.Ignore)) return;
        
        audioSource.PlayOneShot(hitEffect);

        // var target = hit.transform.GetComponent<EnemyManager>();
        // if (target == null) return;
        // var isAwareOfPlayer = target.GetComponent<AIController>().IsLocked();
        // var realDamage = !isAwareOfPlayer ? damage * 2 : damage;
        // target.Health -= realDamage;
        // fpsCam.transform.DOShakeRotation(0.1f, 10f);
    }

    public override void LeaveUse()
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
}
