using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interactable", menuName = "Interactables/Interactable")]
public class InteractableData : ScriptableObject
{
    public string toolTip;
    public string activateToolTip;
    public string deactivateToolTip;
}
