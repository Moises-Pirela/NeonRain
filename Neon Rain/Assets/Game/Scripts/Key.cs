using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public KeyData keyData;
    
    private void OnTriggerEnter(Collider other)
    {
        SaveData.Current.inventory.keyRing.Add(keyData.keyID);
    }
}
