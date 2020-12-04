using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster _current;
    
    private bool isPaused;

    [SerializeField] private GameObject player;

    private GameObject spawnedPlayer;

    public LevelManager _levelManager;

    public Action onRestartLevel;

    //private SceneView sceneView;

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    private void Awake()
    {
        _current = this;
        SaveData.Current = (SaveData) SerializationManager.Load(Application.persistentDataPath + "/saves/save.save");
       // sceneView = SceneView.lastActiveSceneView;
        
    }

    private void Start()
    {
        //Set level start position
        //#if UNITY_EDITOR
       
        //SpawnPlayer(sceneView.camera.transform.position);
        //#else
        if (SaveData.Current.levelsStarted[_levelManager.levelData.levelIndex] && SaveData.Current.lastSpawnPosition != Vector3.zero)
        {
            SpawnPlayer(SaveData.Current.lastSpawnPosition);             
        }
        else
        {
            SpawnPlayer(_levelManager.spawnPosition.position);
        }
        //#endif
        
        
        AIBlackboard._current.Init();
        
        _levelManager.StartLevel();
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        //SpawnPlayer(SaveData.Current.lastSpawnPosition);

        // onRestartLevel.Invoke();
        //
        // _levelManager.RespawnEnemies();
    }

    private void SpawnPlayer(Vector3 spawnPosition)
    {
        if (spawnedPlayer)
        {
            spawnedPlayer.transform.position = spawnPosition;
        }
        else
        {
            spawnedPlayer = Instantiate(player, spawnPosition, Quaternion.identity);            
        }
        
    }
}
