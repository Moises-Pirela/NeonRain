using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModule : Interactable
{
    public override void Interact(PlayerManager playerManager = null, bool magneticInteraction = false)
    {
        SaveData.Current.inventory.upgradeModules++;
        Destroy(gameObject);
    }

    public override void OnHighlight()
    {
    }
}
