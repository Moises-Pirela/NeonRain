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

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        // var left = leftHand.GetComponent<Equipment>();
        // var right = rightHand.GetComponent<Equipment>();
        // EquipLeftHand(left);
        // EquipRightHand(right);
        SelectRHandEquipment();
    }
    
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void SelectRHandEquipment()
    {
        var equipment = 0;
        foreach (Transform currentEquipment in rightHandEquipmentHolder)
        {
            if (equipment == selectedEquipment)
            {
                currentEquipment.gameObject.SetActive(true);
                var right = currentEquipment.GetComponent<Equipment>();
                EquipRightHand(right);
            }
            else
            {
                currentEquipment.gameObject.SetActive(false);    
            } 
            
            equipment++;
        }
    }

    private void Update()
    {
        //if (PlayerEvents.Current.isPaused) return;
        
        // if (rightHandEquipment.MyInput())
        // {
        //     if (grabbedObject != null)
        //     {
        //         //var obj = grabbedObject.GetComponent<SelectableObject>();
        //         //var explosive = grabbedObject.GetComponent<Explosive>();
        //
        //         //PlayerEvents.Current.isGrabbing = false;
        //         
        //         //if (!explosive)
        //             //explosive.shouldExplode = true;
        //
        //         //obj.Throw(fpsCam.transform.forward, 1000);
        //         grabbedObject = null;
        //     }
        //     else
        //     {
        //         rightHandEquipment.Use();       
        //     }
        // }

        // if (leftHandEquipment.MyReverseInput())
        // {
        //     leftHandEquipment.LeaveUse();
        // }
        //
        // if (Input.GetButton("Reload"))
        // {
        //
        // }

        //var prevEquipment = selectedEquipment;

        // if (Input.GetAxis("Mouse ScrollWheel") > 0)
        // {
        //     if (selectedEquipment >= rightHandEquipmentHolder.childCount - 1)
        //         selectedEquipment = 0;
        //     else
        //     {
        //         selectedEquipment++;
        //     }
        // }
        // if (Input.GetAxis("Mouse ScrollWheel") < 0)
        // {
        //     if (selectedEquipment <= 0)
        //         selectedEquipment = rightHandEquipmentHolder.childCount - 1;
        //     else
        //     {
        //         selectedEquipment--;
        //     }
        // }

        // if (Input.GetButtonDown("LeanRight"))
        // {
        //     fpsCam.transform.DORotate(new Vector3(0, 0, -20), 0.2f);
        //     fpsCam.transform.DOMoveX(1.5f, 0.2f);
        // }
        // else if (Input.GetButtonDown("LeanLeft"))
        // {
        //     fpsCam.transform.DORotate(new Vector3(0, 0, 20), 0.2f);
        //     fpsCam.transform.DOMoveX(-1.5f, 0.2f);
        // }
        
        // if (prevEquipment != selectedEquipment)
        //         SelectRHandEquipment();
        
        // if (PlayerEvents.Current.isGrappling && !PlayerEvents.Current.isPulling)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, targetLocation, 14 * Time.deltaTime);
        // }
        // else if (PlayerEvents.Current.isPulling)
        // {
        //     if (targetObject != null)
        //     {
        //         targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, transform.position, 14 * Time.deltaTime);
        //     }
        // }
    }

    public void EquipRightHand(Equipment right)
    {
        rightHandEquipment = right;
        
        rightHandEquipment.SetEquipment(fpsCam, playerController);
        
        _playerControls.Player.Primary_Fire.performed += context => rightHandEquipment.Use();
    }
    
    public void EquipLeftHand(Equipment left)
    {
        leftHandEquipment = left;

        myLeftHandAction.performed -= context => leftHandEquipment.Use();

        leftHandEquipment.SetEquipment(fpsCam, playerController, this);
        
        myLeftHandAction = leftHandEquipment.MyInput();

        myLeftHandAction.performed += context => leftHandEquipment.Use();
    }
}
