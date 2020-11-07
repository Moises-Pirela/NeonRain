using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Interactable _currentInteractable;

    public LayerMask interactableMask;

    public Camera fpsCam;

    public float interactionDistance;

    public PlayerManager playerManager;

    public void Interact()
    {
        if (!_currentInteractable) return; 
        
        _currentInteractable.Interact(playerManager);
    }
    
    public void Update()
    {
        _currentInteractable = null;
        
        if (!Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var hit, interactionDistance, interactableMask)) return;

        var interactable = hit.collider.GetComponent<Interactable>();

        interactable?.OnHighlight();
        _currentInteractable = interactable;
    }
}
