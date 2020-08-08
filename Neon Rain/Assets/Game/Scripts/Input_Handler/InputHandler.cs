using UnityEngine;
using NaughtyAttributes;

namespace VHS
{    
    public class InputHandler : MonoBehaviour
    {
        #region Data
            [Space,Header("Input Data")]
            [SerializeField] private CameraInputData cameraInputData = null;
            [SerializeField] private MovementInputData movementInputData = null;
            [SerializeField] private InteractionInputData interactionInputData = null;
            
            private PlayerControls _playerControls;
        #endregion

        #region BuiltIn Methods
            void Awake()
            {
                
                _playerControls = new PlayerControls();
                _playerControls.Player.Movement.performed += context => movementInputData.InputVector = context.ReadValue<Vector2>();
                _playerControls.Player.Sprint.performed += context =>
                {
                    movementInputData.RunClicked = true;
                    movementInputData.RunReleased = false;
                    movementInputData.IsRunning = true;
                };
                _playerControls.Player.Sprint.canceled += context =>
                {
                    movementInputData.RunReleased = true;
                    movementInputData.RunClicked = false;
                    movementInputData.IsRunning = false;
                };
                _playerControls.Player.Jump.performed += context => movementInputData.JumpClicked = true;
                _playerControls.Player.Crouch.performed += context =>
                {
                    PlayerEvents.Current.Crouch();
                    movementInputData.CrouchClicked = true;
                };
                _playerControls.Player.Look.performed += context => cameraInputData.InputVector = context.ReadValue<Vector2>();
                //_playerControls.Player.Slide.performed += context => m_Slide = true;

                cameraInputData.ResetInput();
                movementInputData.ResetInput();
                interactionInputData.ResetInput();
            }
            private void OnEnable()
            {
                _playerControls.Enable();
            }

            private void OnDisable()
            {
                _playerControls.Disable();
            }
            

            void Update()
            {
                // GetCameraInput();
                // GetMovementInputData();
                // GetInteractionInputData();
            }
        #endregion

        #region Custom Methods
            void GetInteractionInputData()
            {
                interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
                interactionInputData.InteractedReleased = Input.GetKeyUp(KeyCode.E);
            }

            void GetCameraInput()
            {
                cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
                cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

                cameraInputData.ZoomClicked = Input.GetMouseButtonDown(1);
                cameraInputData.ZoomReleased = Input.GetMouseButtonUp(1);
            }

            void GetMovementInputData()
            {
                movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
                movementInputData.InputVectorY = Input.GetAxisRaw("Vertical");

                movementInputData.RunClicked = Input.GetKeyDown(KeyCode.LeftShift);
                movementInputData.RunReleased = Input.GetKeyUp(KeyCode.LeftShift);

                if(movementInputData.RunClicked)
                    movementInputData.IsRunning = true;

                if(movementInputData.RunReleased)
                    movementInputData.IsRunning = false;

                movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
                movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.LeftControl);
            }
        #endregion
    }
}