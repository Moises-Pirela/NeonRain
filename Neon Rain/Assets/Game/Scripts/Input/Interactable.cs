using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractableData data;
    
    public abstract void Interact(PlayerManager playerManager = null, bool magneticInteraction = false);

    public abstract void OnHighlight();
}
