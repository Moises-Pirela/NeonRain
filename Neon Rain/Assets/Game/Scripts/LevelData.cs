using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "New Level", menuName = "Levels/Level")]
public class LevelData : ScriptableObject
{
 public int levelIndex;
 public string levelName;
}
