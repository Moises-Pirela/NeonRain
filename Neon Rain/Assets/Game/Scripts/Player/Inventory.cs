using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.PlayerScripts;
using UnityEngine;

[Serializable]
public class Inventory
{
    public EquipmentData pistol = new EquipmentData(1, 10, 25);
    public EquipmentData gravityAttractor = new EquipmentData(1, 0, 0,25);
    public EquipmentData gravityGrapple = new EquipmentData(-1, 0,0,50);
    public EquipmentData magnetize = new EquipmentData(1, 0,0,50);
    public EquipmentData dagger = new EquipmentData(1, 0, 0, 0);

    public EquipmentData leftHand;
    public EquipmentData rightHand;
    
    public List<int> keyRing = new List<int>(); //key id's

    public int upgradeModules = 0;

    public Inventory()
    {
        rightHand = pistol;
        leftHand = gravityAttractor;
    }
}
