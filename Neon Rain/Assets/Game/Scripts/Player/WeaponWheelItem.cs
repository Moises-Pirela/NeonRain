using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponWheelItem : MonoBehaviour
{
    private Image image;

    public Color32 highlightColor;
    private Color32 normalColor;

    public EquipmentScriptableObject data;

    private void Awake()
    {
        image = GetComponent<Image>();
        normalColor = image.color;
    }

    private void OnEnable()
    {
        Wash();
    }

    public void Wash()
    {
        image.color = normalColor;
    }

    public void Highlight()
    {
        image.color = highlightColor;
    }
    
    public void Select()
    {
        if (data.GetData().equipmentType == EquipmentData.EquipmentType.Lhand)
        {
            SaveData.Current.inventory.leftHand = data.GetData();
        }
        else
        {
            SaveData.Current.inventory.rightHand = data.GetData();
            PlayerHudController.ChangedWeapon = true;
        }
    }
}
