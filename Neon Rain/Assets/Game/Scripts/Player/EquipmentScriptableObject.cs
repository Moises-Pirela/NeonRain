using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Equipment")]
public class EquipmentScriptableObject : ScriptableObject
{
    public EquipmentData myData;
   

    public EquipmentVariant type;

    public EquipmentData GetData()
    {
        switch (type)
        {
            case EquipmentVariant.PISTOL:
                myData = SaveData.Current.inventory.pistol;
                break;
            case EquipmentVariant.DAGGER:
                myData = SaveData.Current.inventory.dagger;
                break;
            case EquipmentVariant.MAGNETIZE:
                myData = SaveData.Current.inventory.magnetize;
                break;
            case EquipmentVariant.GRAPPLE:
                myData = SaveData.Current.inventory.gravityGrapple;
                break;
            case EquipmentVariant.ATTRACTOR:
                myData = SaveData.Current.inventory.gravityAttractor;
                break;
        }

        return myData;
    }
}

public enum EquipmentVariant {PISTOL,DAGGER,MAGNETIZE,GRAPPLE,ATTRACTOR }
