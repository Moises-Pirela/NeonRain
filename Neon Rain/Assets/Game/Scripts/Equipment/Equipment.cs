using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Equipment : MonoBehaviour
{
    public enum EquipmentType {Lhand, Rhand}

    public EquipmentType equipmentType;

    public AudioSource audioSource;
    public AudioClip useEffect;

    public abstract void SetEquipment(Camera camera, Transform player = null, EquipmentController equipmentController = null);

    public abstract void Use();

    public abstract void LeaveUse();

    public abstract InputAction MyInput();

    public abstract bool MyReverseInput();
}