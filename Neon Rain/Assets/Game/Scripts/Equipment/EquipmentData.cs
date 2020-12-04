using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentData
{
    [Serializable]
    public enum EquipmentType
    {
        Lhand,
        Rhand
    };
    
    public EquipmentType equipmentType;
    
    public int level;
    private int ammo;

    public int Ammo
    {
        get => ammo;
        set => ammo =  Mathf.Clamp(value,0,maxAmmo);
    }

    public int maxAmmo;

    public float armorDrain;
    
    
    public bool IsUnlocked => level > 0;

    public EquipmentData( int level = -1, int startingAmmo = 0, int maxAmmo = 0, float armorDrain = 0, EquipmentType type = EquipmentType.Rhand)
    {
        this.level = level;
        Ammo = startingAmmo;
        this.maxAmmo = maxAmmo;
        this.armorDrain = armorDrain;
        equipmentType = type;
    }
}
