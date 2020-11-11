using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Game.Scripts.PlayerScripts
{
    public class GravityAttractor : Equipment
    {
        private Camera fpsCam;
        private Transform player;
        private EquipmentController equipmentController;
        private GameObject lockedObject;

        [SerializeField] private Transform holdPosition;

        [SerializeField] private float range = 15;

        private bool readyToAttract = false;
        private bool attracting = false;

        private Rigidbody target;

        public LayerMask layerMask;
        
        public GravityAttractor(EquipmentData data) : base(data)
        {
        }
        
        private void Awake()
        {
            mydata = SaveData.Current.inventory.gravityAttractor;
        }
        
        public override void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null)
        {
            fpsCam = camera;
            this.player = player;
            this.equipmentController = equipmentController;
        }

        public override void Use(InputAction.CallbackContext context)
        {
            switch (context.interaction)
            {
                case TapInteraction _:
                    Debug.Log("Press");
                    if (target)
                    {
                        ClearTarget();
                        Debug.Log("CLEARED");
                    }
                        
                    break;
                case HoldInteraction _:
                    Debug.Log("Hold");
                    if (_playerManager.Armor < mydata.armorDrain) return;
                    Attract();
                    
                    break;
            }
            
            context.interaction.Reset();
        }

        public override void LeaveUse(InputAction.CallbackContext context)
        {
            
            //
            // switch (context.interaction)
            // {
            //     case TapInteraction _:
            //         
            //         break;
            //     case HoldInteraction _:
            //         Attract();
            //         break;
            // }
        }

        public override InputAction MyInput()
        {
            PlayerControls _playerControls = new PlayerControls();
            return _playerControls.Player.Secondary_Fire;
        }

        public override bool MyReverseInput()
        {
            return Input.GetButtonUp("Fire2");
        }

        private void Update()
        {
            if (target != null && attracting)
            {
                target.transform.position = Vector3.Slerp(target.transform.position, holdPosition.position, 0.3f);
                Debug.Log("Attracting");
            }
        }

        private void Attract()
        {
            FindTarget();
            
            if (!target) return;

            var agent = target.GetComponent<BaseAI>();
            
            Transform targetTransform;

            targetTransform = target.transform;//.DORotate(holdPosition.position, 0.3f);

            if (agent)
            {
                agent.agent.enabled = false;
            }
            else
            {
                target.AddTorque(Vector3.forward * 5f, ForceMode.Impulse);
            }

            targetTransform.parent = holdPosition;

            target.constraints = RigidbodyConstraints.FreezePosition;
            
            readyToAttract = false;

            attracting = true;
            
            _playerManager.DrainArmor(mydata.armorDrain);
            
        }

        private void FindTarget()
        {
            ClearTarget();

            if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, range, layerMask)) return;
           
            Rigidbody hitTarget = hit.collider.GetComponent<Rigidbody>();
            
            if (!hitTarget) return;

            target = hitTarget;
        }

        private void FixedUpdate()
        {
            // if (readyToAttract)
            //     FindTarget();
        }

        private void ClearTarget()
        {
            if (target)
            {
                target.constraints = RigidbodyConstraints.None;
                target.useGravity = false;
                target.transform.parent = null;
                attracting = false;
            }
            
            target = null;
            
        }
    }
}