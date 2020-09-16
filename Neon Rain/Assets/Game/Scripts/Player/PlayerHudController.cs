using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;

    [SerializeField] private BaseEntity player;

    private void Awake()
    {
        player.onArmorChange += UpdateStatusBars;
        player.onHealthChange += UpdateStatusBars;
    }

    private void UpdateStatusBars()
    {
        healthSlider.value = player.Health / player.maxHealth;
        shieldSlider.value = player.Armor / player.maxShield;
    }
}
