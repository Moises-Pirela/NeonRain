using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class Equipment : MonoBehaviour
{
    public EquipmentData mydata;

    public AudioSource audioSource;
    public AudioClip useEffect;

    public PlayerManager _playerManager;

    public RuntimeAnimatorController animatorController ;

    public Vector3 equippedHandPos;

    protected Equipment(EquipmentData data)
    {
        mydata = data;
    }

    public abstract void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null);

    public abstract void Use(InputAction.CallbackContext context);

    public abstract void LeaveUse(InputAction.CallbackContext context);

    public abstract InputAction MyInput();

    public abstract bool MyReverseInput();
}