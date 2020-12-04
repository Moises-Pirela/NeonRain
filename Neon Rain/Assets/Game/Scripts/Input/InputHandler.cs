using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XR.Haptics;

namespace VHS
{
    public enum InputDevices
    {
        CONTROLLER,
        KEYBOARD_MOUSE
    }

    public class InputHandler : MonoBehaviour
    {
        #region Data

        [Space, Header("Input Data")] [SerializeField]
        private CameraInputData cameraInputData = null;

        [SerializeField] public MovementInputData movementInputData = null;
        [SerializeField] private InteractionController _interactionController;

        private PlayerControls _playerControls;

        [SerializeField] private PlayerManager _playerManager;

        public static bool isRumbling;

        public static InputDevices currentDevice;
        public static PlayerInput _controls;

        public const string CONTROLLER_KEY = "Controller";
        public const string KEYBOARD_MOUSE_KEY = "Keyboard and Mouse";

        #endregion

        #region BuiltIn Methods

        void Awake()
        {
            _playerControls = new PlayerControls();

            _controls = GetComponent<PlayerInput>();
            
            InputSystem.pollingFrequency = 120;

            // _controls.onControlsChanged += onInputDeviceChange;
            
            //InputUser.onChange += onInputDeviceChange;

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

            //InputUser.onChange += onInputDeviceChange;
        }

        private void onInputDeviceChange(PlayerInput obj)
        {

            if (obj.currentControlScheme == "Controller")
                currentDevice = InputDevices.CONTROLLER;
            else if (obj.currentControlScheme == "Keyboard and Mouse")
                currentDevice = InputDevices.KEYBOARD_MOUSE;
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            
            // InputUser.onChange += (user, change, arg3) =>
            // {
            //     
            // };
        }

        #endregion

        #region Custom Methods

        public static bool IsController()
        {
            return _controls.currentControlScheme == InputHandler.CONTROLLER_KEY;
        }

        public static IEnumerator Rumble(float leftRumble, float rightRumble, float timer)
        {
            isRumbling = true;
            
            Gamepad.current.SetMotorSpeeds(leftRumble, rightRumble);

            yield return new WaitForSeconds(timer);
            
            InputSystem.ResetHaptics();

            isRumbling = false;    
        }

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

            if (movementInputData.RunClicked)
                movementInputData.IsRunning = true;

            if (movementInputData.RunReleased)
                movementInputData.IsRunning = false;

            movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
            movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.LeftControl);
        }

        #endregion
    }
}