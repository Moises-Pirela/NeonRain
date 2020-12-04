using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using VHS;

public class WeaponWheel : MonoBehaviour
{
    private PlayerControls _playerControls;

    private Vector2 mousePosition;
    private Vector2 moveInput;

    private float cursorAngle;

    private Vector2 screenCenter;

    [SerializeField] private WeaponWheelItem[] items;

    private WeaponWheelItem currentItem;

    private bool left;
    private bool right;

    private bool up;
    private bool down;
    
    private void Awake()
    {
        _playerControls = new PlayerControls();

        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2).normalized;
    }

    private void Start()
    {
        _playerControls.UI.Wheel.performed += context =>
        {
            mousePosition = context.ReadValue<Vector2>();

            if (!InputHandler.IsController())
            {
                moveInput.x = mousePosition.x - (Screen.width / 2f);
                moveInput.y = mousePosition.y - (Screen.height / 2f);
                moveInput.Normalize();    
            }
          
            GetAngle();
            
            if (currentItem != null)
                currentItem.Wash();

            for (int i = 0; i < items.Length; i++)
            {
                if (cursorAngle > i * 72 && cursorAngle < (i + 1) * 72)
                {
                    currentItem = items[i];
                }
            }

            // if (cursorAngle >= 90)
            // {
            //     currentItem = items[0];
            // }
            // else
            // {
            //     currentItem = items[1];
            // }           
            
            if (currentItem != null)
                currentItem.Highlight();

        };
    }

    private void GetAngle()
    {
        if (!InputHandler.IsController())
        {
            cursorAngle = CalculateAngle(moveInput);
        }
        else
        {
            cursorAngle = CalculateAngle(mousePosition);
        }
    }

    private float CalculateAngle(Vector2 angleVector)
    {
        
        var angle = Mathf.Atan2(angleVector.y,-angleVector.x) / Mathf.PI;
        angle *= 180;

        angle += 90f;
         
        if (angle < 0) angle += 360;
        
        
        return angle;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        currentItem.Select();
    }
    
    
    
}
