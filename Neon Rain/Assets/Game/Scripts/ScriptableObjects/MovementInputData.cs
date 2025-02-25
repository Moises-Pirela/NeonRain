﻿using UnityEngine;

namespace VHS
{    
    [CreateAssetMenu(fileName = "MovementInputData", menuName = "FirstPersonController/Data/MovementInputData", order = 1)]
    public class MovementInputData : ScriptableObject
    {
        #region Data
            Vector2 m_inputVector;

            bool m_isRunning = false;
            bool m_isCrouching = false;

            bool m_crouchClicked = false;
            bool m_jumpClicked = false;
            bool m_dash = false;

            bool m_runClicked = false;
            bool m_runReleased = true;
            private bool m_isGrappling = false;
        #endregion

        #region Properties
            public Vector2 InputVector
            {
                get => m_inputVector;
                set => m_inputVector = value;
            }

            public Vector3 DashVector;

            public bool HasInput => m_inputVector != Vector2.zero;
            public float InputVectorX
            {
                set => m_inputVector.x = value;
            }

            public float InputVectorY
            {
                set => m_inputVector.y = value;
            }

            public bool IsRunning
            {
                get => m_isRunning;
                set => m_isRunning = value;
            }

            public bool IsGrappling
            {
                get => m_isGrappling;
                set => m_isGrappling = value;
            }

            public bool IsCrouching
            {
                get => m_isCrouching;
                set => m_isCrouching = value;
            }

            public bool CrouchClicked
            {
                get => m_crouchClicked;
                set => m_crouchClicked = value;
            }

            public bool JumpClicked
            {
                get => m_jumpClicked;
                set => m_jumpClicked = value;
            }

            public bool Dashed
            {
                get => m_dash;
                set => m_dash = value;
            }

            public bool RunClicked
            {
                get => m_runClicked;
                set => m_runClicked = value;
            }

            public bool RunReleased
            {
                get => m_runReleased;
                set => m_runReleased = value;
            }
        #endregion

        #region Custom Methods
            public void ResetInput()
            {
                m_inputVector = Vector2.zero;
                DashVector = Vector3.zero;
                
                m_isRunning = false;
                m_isCrouching = false;

                m_crouchClicked = false;
                m_jumpClicked = false;
                m_runClicked = false;
                m_runReleased =false;
                m_isGrappling = false;
                m_dash = false;
            }
        #endregion
    }
}