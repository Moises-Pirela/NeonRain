using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keys/New Key", fileName = "New Key")]
public class KeyData : ScriptableObject
{
    public string keyName;
    public int keyID;
}
