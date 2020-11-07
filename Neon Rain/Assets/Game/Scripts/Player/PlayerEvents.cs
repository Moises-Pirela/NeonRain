using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static PlayerEvents Current;

    public bool hasAmmo = true;
    public bool isAiming = false;
    public bool isGrappling = false;
    public bool isPulling = false;
    public bool isGrabbing = false;
    public bool isCrouching = false;
    public bool isPaused = false;

    public bool isAttacking = false;
    public bool isSwappingWeapons = false;
    public bool weaponSwapped = false;

    void Awake()
    {
        Current = this;
    }

    public event Action onShake;

    public void Shake()
    {
        onShake?.Invoke();
    }

    public event Action onRest;

    public void Rest()
    {
        onRest?.Invoke();
    }

    public event Action onSwapWeapon;

    public void SwapWeapon()
    {
        onSwapWeapon?.Invoke();
    }

    public event Action onPlayerMelee;

    public void PlayerMelee()
    {
        onPlayerMelee.Invoke();
    }
    
    public event Action onPlayerShoot;
    public void PlayerShoot()
    {
        onPlayerShoot.Invoke();
    }

    public event Action onPlayerWalk;
    public void Walk()
    {
        onPlayerWalk.Invoke();
    }

    public event Action onPlayerStartSprint;
    public void StartSprint()
    {
        onPlayerStartSprint.Invoke();
    }

    public event Action sprinting;

    public void Sprinting()
    {
        sprinting.Invoke();
    }
    
    public event Action onPlayerStopSprint;
    public void StopSprint()
    {
        onPlayerStopSprint.Invoke();
    }

    public event Action onPlayerCrouch;
    public void Crouch()
    {
        isCrouching = true;
        onPlayerCrouch.Invoke();
    }
    
    public event Action onPlayerStand;
    public void Stand()
    {
        isCrouching = false;
        onPlayerStand.Invoke();
    }
}
