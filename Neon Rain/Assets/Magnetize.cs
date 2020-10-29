using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    public override void Use(InputAction.CallbackContext context)
    {
        Debug.Log("USE");
        if (DebugController._instance.IsInConsole || GameMaster._current.IsPaused) return;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, 100, _layerMask,
            QueryTriggerInteraction.Ignore))
        {
            var magnet = hit.collider.GetComponent<Magnet>();
        
            if (magnet == null) return;

            if (magnet.magnetType == MagnetType.Inanimate)
            {
                //Go ahead and use
                var magnetInteractable = magnet.gameObject.GetComponent<Interactable>();
            
                magnetInteractable.Interact();
                return;
            }

            if (targets >= 2 || targetMagnets.Contains(magnet)) return;
            
            targetMagnets.Add(magnet);
            targets++;
            magnet.tagged = true;
            Debug.LogFormat("TARGETS TAGGED : {0}", targets);
        }
        else
        {
            if (targets >= 2)
            {
                Activate();
            }
        }
    }

    public override void LeaveUse(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
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
