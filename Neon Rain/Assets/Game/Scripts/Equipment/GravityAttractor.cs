using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
        public override void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null)
        {
            fpsCam = camera;
            this.player = player;
            this.equipmentController = equipmentController;
        }

        public override void Use()
        {
            var layerMask = 1 << 2;

            layerMask = ~layerMask;
            
            var ray = fpsCam.ScreenPointToRay(Input.mousePosition);
            
            PlayerEvents.Current.PlayerShootGrapple();
            
            if (!audioSource.isPlaying)
                audioSource.Play();;

            if (lockedObject == null)
            {
                if (Physics.Raycast(ray, out var hit, range, layerMask, QueryTriggerInteraction.Ignore)) 
                    lockedObject = hit.collider.gameObject;     
            }
            
            if (lockedObject == null) return;
            
            //equipmentController.targetLocation = hit.point;

            var objectRigidbody = lockedObject.GetComponent<Rigidbody>(); 
            
            if (!objectRigidbody) return;

            objectRigidbody.useGravity = false;
            
            objectRigidbody.AddTorque(new Vector3(1,0.2f,1)* 2);
            objectRigidbody.useGravity = false;
            
            var transform1 = fpsCam.transform;
            var newPosition = transform1.position + transform1.forward * 5;

            objectRigidbody.position = Vector3.Lerp(objectRigidbody.position, newPosition, 5 * Time.deltaTime);

             var enemyAi = lockedObject.GetComponent<IBehaviorAI>();
             if (enemyAi == null) return;
             enemyAi.GetNavMeshAgent().enabled = false;
             
        }

        public override void LeaveUse()
        {
            lockedObject = null;
            
            if (audioSource.isPlaying)
                audioSource.Stop();
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
    }
}