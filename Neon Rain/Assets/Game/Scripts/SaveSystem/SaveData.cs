using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Scripts.PlayerScripts;
using UnityEngine;

[Serializable]
public class SaveData
{
    private static SaveData _current;

    public static SaveData Current {
        get
        {
            if (_current == null)
                _current = new SaveData();
            
            return _current;
        }
        set => _current = value;
    }

    public Vector3 playerPosition;
    public Quaternion playerRotation;
    
    public Vector3 lastSpawnPosition;

    public bool[] levelsStarted = new bool[4];
    
    public List<EntityData> entityData = new List<EntityData>();

    public Inventory inventory = new Inventory();
}