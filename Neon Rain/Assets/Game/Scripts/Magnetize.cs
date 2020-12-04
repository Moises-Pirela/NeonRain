using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Magnetize : Equipment
{
    private Camera fpsCam;

    private List<Magnet> targetMagnets = new List<Magnet>();

    private int targets;

    [SerializeField] private LayerMask _layerMask;
    
    private void Awake()
    {
        mydata = SaveData.Current.inventory.magnetize;
    }
    
    public Magnetize(EquipmentData data) : base(data)
    {
    }

    private void Activate()
    {
        var firstMagnet = targetMagnets[0].GetComponent<Rigidbody>();
        var secondMagnet = targetMagnets[1].GetComponent<Rigidbody>();
        
        var positionA = secondMagnet.transform.position;
        var positionB = firstMagnet.transform.position;
        
        var directionA = positionA - positionB;
        var directionB = positionB - positionA;
        
        firstMagnet.AddForce(directionA * 5, ForceMode.Impulse);
        secondMagnet.AddForce(directionB * 5, ForceMode.Impulse);
        
        targets = 0;
        targetMagnets.Clear();
        
        Debug.Log("Activate");
    }

    public override void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null)
    {
        fpsCam = camera;
        targetMagnets = new List<Magnet>();
        targets = 0;
        targetMagnets.Clear();
    }

    public override void Use(InputAction.CallbackContext context)
    {
        switch (context.interaction)
        {
            case TapInteraction _:
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, 100, _layerMask,
                    QueryTriggerInteraction.Ignore))
                {
                    var magnet = hit.collider.GetComponent<Magnet>();
        
                    if (magnet == null) return;

                    if (magnet.magnetType == MagnetType.Inanimate)
                    {
                        //Go ahead and use
                        var magnetInteractable = magnet.gameObject.GetComponent<Interactable>();
                
                        if (_playerManager.Armor < mydata.armorDrain) return;
                
                        _playerManager.DrainArmor(mydata.armorDrain);
            
                        magnetInteractable.Interact(null,true);
                        return;
                    }

                    if (targets >= 2 || targetMagnets.Contains(magnet)) return;
            
                    targetMagnets.Add(magnet);
                    targets++;
                    magnet.tagged = true;
                }
                // else
                // {
                    if (targets >= 2 && targetMagnets.Count >= 2)
                    {
                        if (_playerManager.Armor < mydata.armorDrain) return;
                        _playerManager.DrainArmor(mydata.armorDrain);
                        Activate();
                    }
                //}
                break;
            case HoldInteraction _:
                
                break;
        }
    }

    public override void CancelUse(InputAction.CallbackContext context)
    {
        
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        switch (context.interaction)
        {
            case TapInteraction _:
                // if (targets >= 2)
                // {
                //     if (_playerManager.Armor < mydata.armorDrain) return;
                //
                //     _playerManager.DrainArmor(mydata.armorDrain);
                //     Activate();
                // }
                break;
            case HoldInteraction _:
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
}
