using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    public Door door;
    
    public override void Interact(PlayerManager playerManager = null, bool magneticInteraction = false)
    {
        door.Interact(playerManager, magneticInteraction);
    }

    public override void OnHighlight()
    {
        door.OnHighlight();
    }
}
