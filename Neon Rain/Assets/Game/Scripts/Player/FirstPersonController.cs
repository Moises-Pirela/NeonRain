using System;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace VHS
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Variables
            #region Private Serialized     
                #region Data
                    [Space, Header("Data")]
                    public MovementInputData movementInputData = null;
                    [SerializeField] private HeadBobData headBobData = null;
                    private AudioSource m_AudioSource;
                    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
                    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
                    [SerializeField] private AudioClip m_LandSound;
                    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
                    [SerializeField] private float m_StepInterval;

                #endregion
                    
                #region Locomotion
                    [Space, Header("Locomotion Settings")]
                    [SerializeField] private float crouchSpeed = 1f;
                    [SerializeField] private float walkSpeed = 2f;
                    [SerializeField] private float runSpeed = 3f;
                    [SerializeField] private float dashSpeed = 20f;
                    [SerializeField] private float jumpSpeed = 5f;
                    [Slider(0f,1f)][SerializeField] private float moveBackwardsSpeedPercent = 0.5f;
                    [Slider(0f,1f)][SerializeField] private float moveSideSpeedPercent = 0.75f;
                #endregion

                #region Run Settings
                    [Space, Header("Run Settings")]
                    [Slider(-1f,1f)][SerializeField] private float canRunThreshold = 0.8f;
                    [SerializeField] private AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
                #endregion

                #region Dash Settings
                [Space, Header("Run Settings")]
                [SerializeField] private AnimationCurve dashTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
                #endregion

                #region Crouch Settings
                    [Space, Header("Crouch Settings")]
                    [Slider(0.2f,0.9f)][SerializeField] private float crouchPercent = 0.6f;
                    [SerializeField] private float crouchTransitionDuration = 1f;
                    [SerializeField] private AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
                #endregion

                #region Landing Settings
                    [Space, Header("Landing Settings")]
                    [Slider(0.05f,0.5f)][SerializeField] private float lowLandAmount = 0.1f;
                    [Slider(0.2f,0.9f)][SerializeField] private float highLandAmount = 0.6f;
                    [SerializeField] private float landTimer = 0.5f;
                    [SerializeField] private float landDuration = 1f;
                    [SerializeField] private AnimationCurve landCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
                #endregion

                #region Gravity
                    [Space, Header("Gravity Settings")]
                    [SerializeField] private float gravityMultiplier = 2.5f;
                    [SerializeField] private float stickToGroundForce = 5f;
                    
                    [SerializeField] private LayerMask groundLayer = ~0;
                    [Slider(0f,1f)][SerializeField] private float rayLength = 0.1f;
                    [Slider(0.01f,1f)][SerializeField] private float raySphereRadius = 0.1f;
                #endregion

                #region Wall Settings
                    [Space, Header("Check Wall Settings")]
                    [SerializeField] private LayerMask obstacleLayers = ~0;
                    [Slider(0f,1f)][SerializeField] private float rayObstacleLength = 0.1f;
                    [Slider(0.01f,1f)][SerializeField] private float rayObstacleSphereRadius = 0.1f;
                    
                #endregion

                #region Smooth Settings
                    [Space, Header("Smooth Settings")]                
                    [Range(1f,100f)] [SerializeField] private float smoothRotateSpeed = 5f;
                    [Range(1f,100f)] [SerializeField] private float smoothInputSpeed = 5f;
                    [Range(1f,100f)] [SerializeField] private float smoothVelocitySpeed = 5f;
                    [Range(1f,100f)] [SerializeField] private float smoothFinalDirectionSpeed = 5f;
                    [Range(1f,100f)] [SerializeField] private float smoothHeadBobSpeed = 5f;

                    [Space]
                    [SerializeField] private bool experimental = false;
                    [InfoBox("It should smooth our player movement to not start fast and not stop fast but it's somehow jerky", InfoBoxType.Warning)]
                    [Tooltip("If set to very high it will stop player immediately after releasing input, otherwise it just another smoothing to our movement to make our player not move fast immediately and not stop immediately")]
                    [ShowIf("experimental")] [Range(1f,100f)] [SerializeField] private float smoothInputMagnitudeSpeed = 5f;
                    
                #endregion
            #endregion
            #region Private Non-Serialized
                #region Components / Custom Classes / Caches
                    private CharacterController m_characterController;
                    private Transform m_yawTransform;
                    private Transform m_camTransform;
                    private HeadBob m_headBob;
                    private CameraController m_cameraController;
                    private PlayerManager _playerManager;
                    
                    private RaycastHit m_hitInfo;
                    private IEnumerator m_CrouchRoutine;
                    private IEnumerator m_LandRoutine;
                #endregion

                #region Debug
                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector2 m_inputVector;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector2 m_smoothInputVector;

                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector3 m_finalMoveDir;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector3 m_smoothFinalMoveDir;
                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector3 m_finalMoveVector;

                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_currentSpeed;
                    
                    public float MCurrentSpeed
                    {
                        get => m_currentSpeed;
                        set => m_currentSpeed = value;
                    }

                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_smoothCurrentSpeed;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_finalSmoothCurrentSpeed;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_walkRunSpeedDifference;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_dashSpeedDifference;

                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_finalRayLength;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private bool m_hitWall;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private bool m_isGrounded;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private bool m_previouslyGrounded;

                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_initHeight;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_crouchHeight;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector3 m_initCenter;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private Vector3 m_crouchCenter;
                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_initCamHeight;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_crouchCamHeight;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_crouchStandHeightDifference;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private bool m_duringCrouchAnimation;
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private bool m_duringRunAnimation;
                    [Space]
                    [BoxGroup("DEBUG")][SerializeField][ReadOnly] private float m_inAirTimer;

                    [Space]
                    [BoxGroup("DEBUG")][ShowIf("experimental")][SerializeField][ReadOnly] private float m_inputVectorMagnitude;
                    [BoxGroup("DEBUG")][ShowIf("experimental")][SerializeField][ReadOnly] private float m_smoothInputVectorMagnitude;
                    
                    private float m_StepCycle;
                    private float m_NextStep;
                    
                #endregion
            #endregion
        
        #endregion

        #region BuiltIn Methods     
            protected virtual void Start()
            {
                GetComponents();
                InitVariables();
            }

            protected virtual void Update()
            {
                if (DebugController._instance.IsInConsole) return;
                
                if(m_yawTransform != null)
                    RotateTowardsCamera();

                if(m_characterController)
                {
                    // Check if Grounded,Wall etc
                    CheckIfGrounded();
                    CheckIfWall();
                    
                    CalculateSpeed();

                    // Apply Smoothing
                    SmoothInput();
                    SmoothSpeed();
                    SmoothDir();

                    if(experimental)
                        SmoothInputMagnitude();

                    // Calculate Movement
                    CalculateMovementDirection();
                    
                    CalculateFinalMovement();

                    // Handle Player Movement, Gravity, Jump, Crouch etc.
                    HandleCrouch();
                    HandleHeadBob();
                    HandleFOV();
                    HandleCameraSway();
                    HandleLanding();
                    HandleDash();

                    ApplyGravity();
                    
                    ApplyMovement();

                    m_previouslyGrounded = m_isGrounded;
                }
            }

            protected void FixedUpdate()
            {
                ProgressStepCycle(m_smoothCurrentSpeed);
            }

            /*
            
                private void OnDrawGizmos()
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere((transform.position + m_characterController.center) - Vector3.up * m_finalRayLength, raySphereRadius);
                }
            
             */
            
        #endregion

        #region Custom Methods
            #region Initialize Methods    
                protected virtual void GetComponents()
                {
                    m_AudioSource = GetComponent<AudioSource>();
                    m_characterController = GetComponent<CharacterController>();
                    m_cameraController = GetComponentInChildren<CameraController>();
                    m_yawTransform = m_cameraController.transform;
                    m_camTransform = GetComponentInChildren<Camera>().transform;
                    m_headBob = new HeadBob(headBobData, moveBackwardsSpeedPercent, moveSideSpeedPercent);
                    _playerManager = GetComponent<PlayerManager>();
                }

                protected virtual void InitVariables()
                {   
                    // Calculate where our character center should be based on height and skin width
                    m_characterController.center = new Vector3(0f,m_characterController.height / 2f + m_characterController.skinWidth,0f);

                    m_initCenter = m_characterController.center;
                    m_initHeight = m_characterController.height;

                    m_crouchHeight = m_initHeight * crouchPercent;
                    m_crouchCenter = (m_crouchHeight / 2f + m_characterController.skinWidth) * Vector3.up;

                    m_crouchStandHeightDifference = m_initHeight - m_crouchHeight;

                    m_initCamHeight = m_yawTransform.localPosition.y;
                    m_crouchCamHeight = m_initCamHeight - m_crouchStandHeightDifference;

                    // Sphere radius not included. If you want it to be included just decrease by sphere radius at the end of this equation
                    m_finalRayLength = rayLength + m_characterController.center.y;

                    m_isGrounded = true;
                    m_previouslyGrounded = true;

                    m_inAirTimer = 0f;
                    m_headBob.CurrentStateHeight = m_initCamHeight;

                    m_walkRunSpeedDifference = runSpeed - walkSpeed;
                    
                    m_StepCycle = 0f;
                    m_NextStep = m_StepCycle/2f;
                }
                
                private void PlayLandingSound()
                {
                    m_AudioSource.clip = m_LandSound;
                    m_AudioSource.Play();
                    m_NextStep = m_StepCycle + .5f;
                }
                
                private void PlayJumpSound()
                {
                    m_AudioSource.clip = m_JumpSound;
                    m_AudioSource.Play();
                }
                private void ProgressStepCycle(float speed)
                {
                    if (m_characterController.velocity.sqrMagnitude > 0 && (m_inputVector.x != 0 || m_inputVector.y != 0))
                    {
                        var stepModifier = movementInputData.IsCrouching ? 3f : 1;
                        m_StepCycle += (m_characterController.velocity.magnitude + (speed*(!movementInputData.IsRunning ? stepModifier : m_RunstepLenghten)))*
                                       Time.fixedDeltaTime;
                    }

                    if (!(m_StepCycle > m_NextStep))
                    {
                        return;
                    }

                    m_NextStep = m_StepCycle + m_StepInterval;

                    PlayFootStepAudio();
                }
                
                
                private void PlayFootStepAudio()
                {
                    if (!m_characterController.isGrounded)
                    {
                        return;
                    }
                    // pick & play a random footstep sound from the array,
                    // excluding sound at index 0
                    int n = Random.Range(1, m_FootstepSounds.Length);
                    m_AudioSource.clip = m_FootstepSounds[n];
                    m_AudioSource.PlayOneShot(m_AudioSource.clip);
                    // move picked sound to index 0 so it's not picked next time
                    m_FootstepSounds[n] = m_FootstepSounds[0];
                    m_FootstepSounds[0] = m_AudioSource.clip;
                }
            #endregion

            #region Smoothing Methods
                protected virtual void SmoothInput()
                {
                    m_inputVector = movementInputData.InputVector.normalized;
                    m_smoothInputVector = Vector2.Lerp(m_smoothInputVector,m_inputVector,Time.deltaTime * smoothInputSpeed);
                    //Debug.DrawRay(transform.position, new Vector3(m_smoothInputVector.x,0f,m_smoothInputVector.y), Color.green);
                }

                protected virtual void SmoothSpeed()
                {
                    m_smoothCurrentSpeed = Mathf.Lerp(m_smoothCurrentSpeed, m_currentSpeed, Time.deltaTime * smoothVelocitySpeed);

                    if(movementInputData.IsRunning && CanRun())
                    {
                        float _walkRunPercent = Mathf.InverseLerp(walkSpeed,runSpeed, m_smoothCurrentSpeed);
                        m_finalSmoothCurrentSpeed = runTransitionCurve.Evaluate(_walkRunPercent) * m_walkRunSpeedDifference + walkSpeed;
                    }
                    else
                    {
                        m_finalSmoothCurrentSpeed = m_smoothCurrentSpeed;
                    }
                }

                protected virtual void SmoothDir()
                {

                    m_smoothFinalMoveDir = Vector3.Lerp(m_smoothFinalMoveDir, m_finalMoveDir, Time.deltaTime * smoothFinalDirectionSpeed);
                    Debug.DrawRay(transform.position, m_smoothFinalMoveDir, Color.yellow);
                }
                
                protected virtual void SmoothInputMagnitude()
                {
                    m_inputVectorMagnitude = m_inputVector.magnitude;
                    m_smoothInputVectorMagnitude = Mathf.Lerp(m_smoothInputVectorMagnitude, m_inputVectorMagnitude, Time.deltaTime * smoothInputMagnitudeSpeed);
                }
            #endregion

             #region Locomotion Calculation Methods
                protected virtual void CheckIfGrounded()
                {
                    Vector3 _origin = transform.position + m_characterController.center;

                    bool _hitGround = m_characterController.isGrounded; //Physics.SphereCast(_origin,raySphereRadius,Vector3.down,out m_hitInfo,m_finalRayLength,groundLayer);
                    Debug.DrawRay(_origin,Vector3.down * (m_finalRayLength),Color.red);

                    m_isGrounded = _hitGround ? true : false;
                }

                protected virtual void CheckIfWall()
                {
                    
                    Vector3 _origin = transform.position + m_characterController.center;
                    RaycastHit _wallInfo;

                    bool _hitWall = false;

                    if(movementInputData.HasInput && m_finalMoveDir.sqrMagnitude > 0)
                        _hitWall = Physics.SphereCast(_origin,rayObstacleSphereRadius,m_finalMoveDir, out _wallInfo,rayObstacleLength,obstacleLayers);
                    Debug.DrawRay(_origin,m_finalMoveDir * rayObstacleLength,Color.blue);

                    m_hitWall = _hitWall ? true : false;
                }

                protected virtual bool CheckIfRoof()
                {
                    var originVector = transform.position;

                    var hitRoof = Physics.SphereCast(originVector,raySphereRadius,Vector3.up,out _,m_initHeight, obstacleLayers);

                    return hitRoof;
                }

                protected virtual bool CanRun()
                {
                    var normalizedDirection = Vector3.zero;

                    if(m_smoothFinalMoveDir != Vector3.zero)
                        normalizedDirection = m_smoothFinalMoveDir.normalized;

                    // ReSharper disable once SuggestVarOrType_BuiltInTypes
                    float dot = Vector3.Dot(transform.forward,normalizedDirection);
                    
                    return dot >= canRunThreshold && !movementInputData.IsCrouching && !movementInputData.Dashed;
                }

                protected virtual void CalculateMovementDirection()
                {
                    var movementTransform = transform;
                    
                    var vDirection = movementTransform.forward * m_smoothInputVector.y;
                    var hDirection = movementTransform.right * m_smoothInputVector.x;

                    var desiredDirection = vDirection + hDirection;
                    var flattenDirection = FlattenVectorOnSlopes(desiredDirection);

                    m_finalMoveDir = flattenDirection;
                }

                protected virtual Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat)
                {
                    if(m_isGrounded)
                        vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat,m_hitInfo.normal);
                    
                    return vectorToFlat;
                }

                protected virtual void CalculateSpeed()
                {
                    m_currentSpeed = movementInputData.IsRunning && CanRun() ? runSpeed : walkSpeed;
                    m_currentSpeed = movementInputData.IsCrouching ? crouchSpeed : m_currentSpeed;
                    m_currentSpeed = !movementInputData.HasInput ? 0f : m_currentSpeed;
                    m_currentSpeed = movementInputData.Dashed ?  dashSpeed : m_currentSpeed;
                    m_currentSpeed = Math.Abs(movementInputData.InputVector.y - (-1)) < 1 ? m_currentSpeed * moveBackwardsSpeedPercent : m_currentSpeed;
                    m_currentSpeed = movementInputData.InputVector.x != 0 && movementInputData.InputVector.y ==  0 ? m_currentSpeed * moveSideSpeedPercent :  m_currentSpeed;
                }

                protected virtual void CalculateFinalMovement()
                {
                    var smoothInputVectorMagnitude = experimental ? m_smoothInputVectorMagnitude : 1f;
                    var finalVector = m_smoothFinalMoveDir * (m_finalSmoothCurrentSpeed * smoothInputVectorMagnitude);

                    m_finalMoveVector.x = finalVector.x ;
                    m_finalMoveVector.z = finalVector.z ;

                    if(m_characterController.isGrounded)
                        m_finalMoveVector.y += finalVector.y ;
                }
            #endregion

            #region Crouching Methods
                protected virtual void HandleCrouch()
                {
                    if (!movementInputData.CrouchClicked || !m_isGrounded) return;
                    
                    InvokeCrouchRoutine();
                    movementInputData.CrouchClicked = false;
                }

                protected virtual void InvokeCrouchRoutine()
                {
                    if (movementInputData.IsCrouching)
                        if (CheckIfRoof())
                            return;

                    if(m_LandRoutine != null)
                        StopCoroutine(m_LandRoutine);

                    if(m_CrouchRoutine != null)
                        StopCoroutine(m_CrouchRoutine);

                    m_CrouchRoutine = CrouchRoutine();
                    StartCoroutine(m_CrouchRoutine);
                }

                protected virtual IEnumerator CrouchRoutine()
                {
                    m_duringCrouchAnimation = true;

                    var percent = 0f;
                    var speed = 1f / crouchTransitionDuration;

                    var currentHeight = m_characterController.height;
                    var currentCenter = m_characterController.center;

                    var desiredHeight = movementInputData.IsCrouching ? m_initHeight : m_crouchHeight;
                    var desiredCenter = movementInputData.IsCrouching ? m_initCenter : m_crouchCenter;

                    var camPos = m_yawTransform.localPosition;
                    var camCurrentHeight = camPos.y;
                    var camDesiredHeight = movementInputData.IsCrouching ? m_initCamHeight : m_crouchCamHeight;
                    
                    
                    movementInputData.IsCrouching = !movementInputData.IsCrouching;
                    m_headBob.CurrentStateHeight = movementInputData.IsCrouching ? m_crouchCamHeight : m_initCamHeight;
                    

                    while(percent < 1f)
                    {
                        percent += Time.deltaTime * speed;
                        var smoothPercent = crouchTransitionCurve.Evaluate(percent);

                        m_characterController.height = Mathf.Lerp(currentHeight,desiredHeight,smoothPercent);
                        m_characterController.center = Vector3.Lerp(currentCenter,desiredCenter,smoothPercent);

                        camPos.y = Mathf.Lerp(camCurrentHeight,camDesiredHeight, smoothPercent);
                        m_yawTransform.localPosition = camPos;

                        yield return null;
                    }

                    m_duringCrouchAnimation = false;
                }
                
            #endregion

            #region Landing Methods
                protected virtual void HandleLanding()
                {
                    if (m_previouslyGrounded || !m_isGrounded || movementInputData.Dashed) return;
                    
                    InvokeLandingRoutine();
                    PlayLandingSound();
                }

                protected virtual void InvokeLandingRoutine()
                {
                    if(m_LandRoutine != null)
                        StopCoroutine(m_LandRoutine);

                    m_LandRoutine = LandingRoutine();
                    StartCoroutine(m_LandRoutine);
                }

                protected virtual IEnumerator LandingRoutine()
                {
                    var percent = 0f;
                    var landAmount = 0f;

                    var speed = 1f / landDuration;

                    var localPos = m_yawTransform.localPosition;
                    var initLandHeight = localPos.y;

                    landAmount = m_inAirTimer > landTimer ? highLandAmount : lowLandAmount;

                    while(percent < 1f)
                    {
                        percent += Time.deltaTime * speed;
                        var desiredY = landCurve.Evaluate(percent) * landAmount;

                        localPos.y = initLandHeight + desiredY;
                        m_yawTransform.localPosition = localPos;

                        yield return null;
                    }
                }
            #endregion

            #region Locomotion Apply Methods

                protected virtual void HandleHeadBob()
                {
                    
                    if(movementInputData.HasInput && m_isGrounded  && !m_hitWall)
                    {
                        if (m_duringCrouchAnimation) return;
                        
                        m_headBob.ScrollHeadBob(movementInputData.IsRunning && CanRun(),movementInputData.IsCrouching, movementInputData.InputVector);
                        m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition,(Vector3.up * m_headBob.CurrentStateHeight) + m_headBob.FinalOffset,Time.deltaTime * smoothHeadBobSpeed);
                    }
                    else
                    {
                        if(!m_headBob.Resetted)
                        {
                            m_headBob.ResetHeadBob();
                        }

                        if(!m_duringCrouchAnimation)
                            m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition,new Vector3(0f,m_headBob.CurrentStateHeight,0f),Time.deltaTime * smoothHeadBobSpeed);
                    }
                }

                protected virtual void HandleCameraSway()
                {
                    m_cameraController.HandleSway(m_smoothInputVector,movementInputData.InputVector.x);
                }

                protected virtual void HandleFOV()
                {
                    if((movementInputData.HasInput || movementInputData.Dashed) && m_isGrounded  && !m_hitWall)
                    {
                        if(movementInputData.RunClicked && CanRun())
                        {
                            m_duringRunAnimation = true;
                            m_cameraController.ChangeFOV(false);
                        }

                        if(movementInputData.IsRunning && CanRun() && !m_duringRunAnimation )
                        {
                            m_duringRunAnimation = true;
                            m_cameraController.ChangeFOV(false);
                        }

                        if (movementInputData.RunClicked && !CanRun())
                        {
                            m_duringRunAnimation = false;
                            m_cameraController.ChangeFOV(true);
                        }
                        
                        if (movementInputData.Dashed)
                            m_cameraController.ChangeFOV(false);
                        
                        if (!movementInputData.Dashed)
                            m_cameraController.ChangeFOV(true);
                    }

                    if (!movementInputData.RunReleased && movementInputData.HasInput && !m_hitWall) return;
                    
                    if (!m_duringRunAnimation) return;
                        
                    m_duringRunAnimation = false;
                    m_cameraController.ChangeFOV(true);
                }
                protected virtual void HandleJump()
                {
                    if (!movementInputData.JumpClicked || movementInputData.IsCrouching) return;
                    
                    m_finalMoveVector.y = jumpSpeed;
                    
                    m_previouslyGrounded = true;
                    m_isGrounded = false;
                        
                    PlayJumpSound();
                }

                protected virtual void HandleDash()
                {
                    if (!movementInputData.Dashed) return;

                    m_finalMoveVector = movementInputData.DashVector;
                    m_finalMoveVector.z *= dashSpeed; 
                    m_finalMoveVector.x *= dashSpeed;
                    m_finalMoveVector.y *= dashSpeed * 0.1f;
                       
                    m_previouslyGrounded = true;
                    m_isGrounded = false;
                        
                }
                protected virtual void ApplyGravity()
                {
                    if (movementInputData.Dashed) return;
                    
                    if(m_characterController.isGrounded) // if we would use our own m_isGrounded it would not work that good, this one is more precise
                    {
                        m_inAirTimer = 0f;
                        m_finalMoveVector.y = -stickToGroundForce;

                        HandleJump();
                        movementInputData.JumpClicked = false;
                    }
                    else
                    {   
                        m_inAirTimer += Time.deltaTime;
                        m_finalMoveVector += Physics.gravity * gravityMultiplier * Time.deltaTime;
                    }
                }

                protected virtual void ApplyMovement()
                {
                    if (!m_characterController.enabled) return;
                    
                    m_characterController.Move(m_finalMoveVector * Time.deltaTime);
                    
                    if (movementInputData.HasInput)
                        PlayerEvents.Current.Walk();
                    if (movementInputData.IsRunning && !movementInputData.IsCrouching)
                    {
                        PlayerEvents.Current.Sprinting();
                    }
                }

                protected virtual void RotateTowardsCamera()
                {
                    var currentRot = transform.rotation;
                    var desiredRot = m_yawTransform.rotation;

                    transform.rotation = Quaternion.Slerp(currentRot,desiredRot,Time.deltaTime * smoothRotateSpeed);
                }
            #endregion
        #endregion
    }
}
