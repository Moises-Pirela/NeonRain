using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : Interactable
{
    public Elevator elevator;
    
    public override void Interact(PlayerManager playerManager = null, bool magneticInteraction = false)
    {
       elevator.Interact();
    }

    public override void OnHighlight()
    {
        elevator.OnHighlight();
    }
}
