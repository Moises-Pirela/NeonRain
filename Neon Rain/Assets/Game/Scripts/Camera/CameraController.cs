﻿using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace VHS
{
    public class CameraController : MonoBehaviour
    {
        #region Variables

        #region Data

        [Space, Header("Data")] [SerializeField]
        private CameraInputData camInputData = null;

        [Space, Header("Custom Classes")] [SerializeField]
        private CameraZoom cameraZoom = null;

        [SerializeField] private CameraSwaying cameraSway = null;
        [SerializeField] private PostProcessVolume _postProcessVolume;
        [SerializeField] private MovementInputData _movementInputData;
        private Vignette _vignette;

        public CameraRecoil recoil;

        #endregion

        #region Settings

        [Space, Header("Look Settings")] [SerializeField]
        private Vector2 sensitivity = Vector2.zero;

        [SerializeField] private Vector2 smoothAmount = Vector2.zero;

        [SerializeField] [MinMaxSlider(-90f, 90f)]
        private Vector2 lookAngleMinMax = Vector2.zero;

        #endregion

        #region Private

        private float m_yaw;
        private float m_pitch;

        private float m_desiredYaw;
        private float m_desiredPitch;

        #region Components

        private Transform m_pitchTranform;
        private Camera m_cam;

        #endregion

        #endregion

        #endregion

        #region BuiltIn Methods

        void Awake()
        {
            GetComponents();
            InitValues();
            InitComponents();
            ChangeCursorState();
        }

        private void Start()
        {
            PlayerEvents.Current.onPlayerCrouch += ApplyCrouchEffect;
            PlayerEvents.Current.onPlayerStand += ApplyCrouchEffect;
        }

        void LateUpdate()
        {
            if (DebugController._instance.IsInConsole) return;

            CalculateRotation();
            SmoothRotation();
            ApplyRotation();
            HandleZoom();
        }

        #endregion

        #region Custom Methods

        private void ApplyCrouchEffect()
        {
            _vignette.active = !_movementInputData.IsCrouching;
        }

        void GetComponents()
        {
            m_pitchTranform = transform.GetChild(0).transform;
            m_cam = GetComponentInChildren<Camera>();
            _postProcessVolume = GameObject.Find("FirstPersonVolume").GetComponent<PostProcessVolume>();

        }

        void InitValues()
        {
            m_yaw = transform.eulerAngles.y;
            m_desiredYaw = m_yaw;
        }

        void InitComponents()
        {
            cameraZoom.Init(m_cam, camInputData);
            cameraSway.Init(m_cam.transform);
            _postProcessVolume.profile.TryGetSettings(out _vignette);
        }

        void CalculateRotation()
        {
            m_desiredYaw += camInputData.InputVector.x * sensitivity.x * Time.deltaTime;
            m_desiredPitch -= camInputData.InputVector.y * sensitivity.y * Time.deltaTime;
            m_desiredPitch = Mathf.Clamp(m_desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
        }

        void SmoothRotation()
        {
            m_yaw = Mathf.Lerp(m_yaw, m_desiredYaw, smoothAmount.x * Time.deltaTime);
            m_pitch = Mathf.Lerp(m_pitch, m_desiredPitch, smoothAmount.y * Time.deltaTime);
        }

        void ApplyRotation()
        {
            transform.eulerAngles = new Vector3(0f, m_yaw, 0f);
            m_pitchTranform.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
        }

        public void HandleSway(Vector3 _inputVector, float _rawXInput)
        {
            cameraSway.SwayPlayer(_inputVector, _rawXInput);
        }

        void HandleZoom()
        {
            if (camInputData.ZoomClicked || camInputData.ZoomReleased)
                cameraZoom.ChangeFOV(this);
        }

        public void ChangeFOV(bool _returning)
        {
            cameraZoom.ChangeRunFOV(_returning, this);
        }

        void ChangeCursorState()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion
    }
}