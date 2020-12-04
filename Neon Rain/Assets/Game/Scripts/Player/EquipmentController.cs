using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public class EquipmentController : MonoBehaviour
{
    private Equipment leftHandEquipment;
    private Equipment rightHandEquipment;
    private GameObject grabbedObject;

    [HideInInspector] public int selectedEquipment = 0;

    [FormerlySerializedAs("rightHandEquipmentHolder")] [SerializeField]
    private Transform equipmentHolder;

    public Camera fpsCam;
    public Transform playerController;

    public PlayerManager _playerManager;

    private InputAction myLeftHandAction;
    private InputAction myRightHandAction;

    private PlayerControls _playerControls;

    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private SpriteRenderer leftHandSprite;
    [SerializeField] private GameObject rainHands;

    public List<Transform> weapons = new List<Transform>();

    private float scrollValue;

    private int _currentWeaponIndex;

    private int CurrentWeaponIndex
    {
        get { return _currentWeaponIndex; }
        set { _currentWeaponIndex = Mathf.Clamp(value, -1, weapons.Count); }
    }

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        foreach (Transform child in equipmentHolder)
        {
            weapons.Add(child);
        }

        _playerControls.Player.Scroll.performed += context =>
        {
            scrollValue = context.ReadValue<float>();
            ScrollSelect(scrollValue);
        };

        _playerControls.Player.Equipment1.performed += context =>
        {
            SelectRightHandEquipment(0); // Pistol
        };
        _playerControls.Player.Equipment2.performed += context =>
        {
            SelectLeftHandEquipment(1); //Grapple
        };
        _playerControls.Player.Equipment3.performed += context =>
        {
            SelectLeftHandEquipment(2); //Grav arm
        };
        _playerControls.Player.Equipment4.performed += context =>
        {
            SelectLeftHandEquipment(3); // Magnetize
        };
        _playerControls.Player.Equipment5.performed += context =>
        {
            SelectRightHandEquipment(4); // Dagger
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
        foreach (Transform currentEquipment in equipmentHolder)
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

    private void ScrollSelect(float val)
    {
        if (val < 0)
        {
            CurrentWeaponIndex--;
            
            if (CurrentWeaponIndex < 0)
            {
                CurrentWeaponIndex = weapons.Count - 1;
            }
        }
        else if (val > 0)
        {
            CurrentWeaponIndex++;

            if (CurrentWeaponIndex > weapons.Count - 1)
            {
                CurrentWeaponIndex = 0;
            }
        }
        
        SelectRightHandEquipment(CurrentWeaponIndex);
        SelectLeftHandEquipment(CurrentWeaponIndex);
    }

    public void SelectRightHandEquipment(int index)
    {
        var selectedEquipment = weapons[index].GetComponent<Equipment>();
        
        if (selectedEquipment.mydata.equipmentType != EquipmentData.EquipmentType.Rhand) return;

        Debug.Log(selectedEquipment.name);

        StartCoroutine(EquipRightHand(selectedEquipment));
    }

    public void SelectLeftHandEquipment(int index)
    {
        var selectedEquipment = weapons[index].GetComponent<Equipment>();
        
        if (selectedEquipment.mydata.equipmentType != EquipmentData.EquipmentType.Lhand) return;

        Debug.Log(selectedEquipment.name);

        EquipLeftHand(selectedEquipment);
    }

    private IEnumerator EquipRightHand(Equipment right)
    {
        if (rightHandEquipment == right) yield break;
        
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

            leftHandEquipment.Use(context);
        };
        
        _playerControls.Player.Cancel.performed += context => leftHandEquipment.CancelUse(context);

        _playerControls.Player.Secondary_Fire.canceled += context => leftHandEquipment.LeaveUse(context);
    }
}