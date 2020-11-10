using System;
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
            [SerializeField] private InteractionController _interactionController;
            
            private PlayerControls _playerControls;
            
            [SerializeField] private PlayerManager _playerManager;
        #endregion

        #region BuiltIn Methods
            void Awake()
            {   
                _playerControls = new PlayerControls();

                _playerControls.Player.Continue.performed += context =>
                {
                    if (!_playerManager.isDead || GameMaster._current.IsPaused) return;

                    _playerManager.isDead = false;
                    //_playerManager.SetUp();
                    
                    GameMaster._current.RestartLevel();
                };
                
                _playerControls.Player.Movement.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                        
                    movementInputData.InputVector = context.ReadValue<Vector2>();
                };
                
                _playerControls.Player.Sprint.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    
                    movementInputData.RunClicked = true;
                    movementInputData.RunReleased = false;
                    movementInputData.IsRunning = true;
                    PlayerEvents.Current.StartSprint();
                };
                
                _playerControls.Player.Sprint.canceled += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    
                    movementInputData.RunReleased = true;
                    movementInputData.RunClicked = false;
                    movementInputData.IsRunning = false;
                    PlayerEvents.Current.StopSprint();
                };
                
                _playerControls.Player.Jump.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    movementInputData.JumpClicked = true;
                };
                
                _playerControls.Player.Crouch.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    
                    if (PlayerEvents.Current.isCrouching) PlayerEvents.Current.Stand();
                    else
                    {
                        PlayerEvents.Current.Crouch();
                    }
                    
                    movementInputData.CrouchClicked = true;
                };
                
                _playerControls.Player.Look.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    
                    cameraInputData.InputVector = context.ReadValue<Vector2>();
                };

                _playerControls.Player.Interact.performed += context =>
                {
                    if (_playerManager.isDead || GameMaster._current.IsPaused) return;
                    _interactionController.Interact();
                };

                cameraInputData.ResetInput();
                movementInputData.ResetInput();
            }

            private void LateUpdate()
            {
                // cameraInputData.ResetInput();
                // movementInputData.ResetInput();
            }

            private void OnEnable()
            {
                _playerControls.Enable();
            }

            private void OnDisable()
            {
                _playerControls.Disable();
            }
        #endregion

        #region Custom Methods
            void GetInteractionInputData()
            {
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