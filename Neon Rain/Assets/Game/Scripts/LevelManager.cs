using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData levelData;

    public Transform spawnPosition;

    public GameObject[] enemies;

    private List<Vector3> _enemyInitialPos = new List<Vector3>();
    private List<Quaternion> _enemyInitialRot = new List<Quaternion>();

    public void StartLevel()
    {
        SaveData.Current.levelsStarted[levelData.levelIndex] = true;

        foreach (var enemy in enemies)
        {
            _enemyInitialPos.Add(enemy.transform.position);
            _enemyInitialRot.Add(enemy.transform.rotation);
        }
    }

    public void RespawnEnemies()
    {
        for (var index = 0; index < enemies.Length; index++)
        {
            var enemy = enemies[index];
            enemy.transform.position = _enemyInitialPos[index];
            enemy.transform.rotation = _enemyInitialRot[index];
            //enemy.GetComponent<AIManager>().Respawn();
            //enemy.GetComponent<BaseAI>().Respawn();
            enemy.SetActive(true);
        }
    }
}
