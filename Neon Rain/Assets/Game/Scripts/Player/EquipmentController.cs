using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class EquipmentController : MonoBehaviour
{
    private Equipment leftHandEquipment;
    private Equipment rightHandEquipment;
    private GameObject grabbedObject;

    [HideInInspector] public int selectedEquipment = 0;

    [SerializeField] private Transform rightHandEquipmentHolder;

    public Camera fpsCam;
    public Transform playerController;

    public PlayerManager _playerManager;

    private InputAction myLeftHandAction;
    private InputAction myRightHandAction;

    private PlayerControls _playerControls;

    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private SpriteRenderer leftHandSprite;
    [SerializeField] private GameObject rainHands;

    private float scrollValue;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        _playerControls.Player.Scroll.performed += context =>
        {
            scrollValue = context.ReadValue<float>();
        };

        PlayerHudController.onCloseWeaponWheel += SelectHandEquipment;
        
        // var left = leftHand.GetComponent<Equipment>();
        // var right = rightHand.GetComponent<Equipment>();
        SelectHandEquipment();
    }
    
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void SelectHandEquipment()
    {
        var equipment = 0;
        foreach (Transform currentEquipment in rightHandEquipmentHolder)
        {
            var selectedEquipment = currentEquipment.GetComponent<Equipment>();
            
            if (selectedEquipment.mydata == SaveData.Current.inventory.rightHand)
            {
                StartCoroutine(EquipRightHand(selectedEquipment));
            }
            else if (selectedEquipment.mydata == SaveData.Current.inventory.leftHand)
            {   
                EquipLeftHand(selectedEquipment);
            }

            equipment++;
        }
    }

    private IEnumerator EquipRightHand(Equipment right)
    {
        PlayerEvents.Current.SwapWeapon();
        
        rightHandEquipment = right;
        
        rightHandEquipment.SetEquipment(fpsCam, playerController);
        
        _playerControls.Player.Primary_Fire.performed += context =>
        {
            if (_playerManager.isDead) return;
            rightHandEquipment.Use(context);
        };
        
        yield return new WaitWhile(() => PlayerEvents.Current.isSwappingWeapons);
        
        rightHandAnimator.runtimeAnimatorController = rightHandEquipment.animatorController;

        yield return new WaitUntil(() => PlayerEvents.Current.weaponSwapped);
        
        rightHandAnimator.gameObject.transform.localPosition = rightHandEquipment.equippedHandPos;
        
        PlayerEvents.Current.isAttacking = false;
    }
    
    public void EquipLeftHand(Equipment left)
    {
        //myLeftHandAction.performed -= context => leftHandEquipment.Use();

        leftHandEquipment = left;

        leftHandEquipment.SetEquipment(fpsCam, playerController, this);
        
        _playerControls.Player.Secondary_Fire.performed += context =>
        {
            if (_playerManager.isDead) return;
            
            if (_playerManager.Armor < left.mydata.armorDrain) return;
            
            leftHandEquipment.Use(context);
        };
        
        _playerControls.Player.Secondary_Fire.canceled += context => leftHandEquipment.LeaveUse(context);
    }
}
