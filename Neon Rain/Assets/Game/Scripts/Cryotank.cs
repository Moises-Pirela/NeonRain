using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class Cryotank : Interactable
{
    public Transform spawnPosition;

    public override void Interact(PlayerManager playerManager)
    {
        StartCoroutine(Interaction(playerManager));
    }

    public IEnumerator Interaction(PlayerManager playerManager)
    {
        PlayerEvents.Current.Rest();
        
        SaveData.Current.lastSpawnPosition = spawnPosition.position;
        SaveData.Current.playerRotation = spawnPosition.rotation;
        
        SerializationManager.Save("save", SaveData.Current);
        
        yield return new WaitForSeconds(1f);
        
        GameMaster._current._levelManager.RespawnEnemies();

        playerManager.Health = playerManager.maxHealth;
        playerManager.Armor = playerManager.maxShield;
    }
}
