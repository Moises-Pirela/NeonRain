using System;
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
    [SerializeField] private float maxGrabDistance;

    [HideInInspector] public Vector3 targetLocation;
    [HideInInspector] public GameObject targetObject;

    public Camera fpsCam;
    public Transform playerController;

    private InputAction myLeftHandAction;
    private InputAction myRightHandAction;

    private PlayerControls _playerControls;

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
            
            Debug.Log(scrollValue);
            
            
        };
        // var left = leftHand.GetComponent<Equipment>();
        // var right = rightHand.GetComponent<Equipment>();
        // EquipLeftHand(left);
        // EquipRightHand(right);
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

            if (selectedEquipment.equipmentType == Equipment.EquipmentType.Rhand)
                EquipRightHand(selectedEquipment);
            else
            {
                EquipLeftHand(selectedEquipment);
            }

            equipment++;
        }
    }

    public void EquipRightHand(Equipment right)
    {
        rightHandEquipment = right;
        
        rightHandEquipment.SetEquipment(fpsCam, playerController);
        
        _playerControls.Player.Primary_Fire.performed += context => rightHandEquipment.Use();
    }
    
    public void EquipLeftHand(Equipment left)
    {
        //myLeftHandAction.performed -= context => leftHandEquipment.Use();

        leftHandEquipment = left;

        leftHandEquipment.SetEquipment(fpsCam, playerController, this);
        
        _playerControls.Player.Secondary_Fire.performed += context => leftHandEquipment.Use();
    }
}
