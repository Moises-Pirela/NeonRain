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

    public bool canShoot = true;

    void Awake()
    {
        Current = this;
    }

    public event Action onKill;
    public void Kill()
    {
        onKill?.Invoke();
    }

    public event Action onArmorChange;
    public void ArmorChange()
    {
        onArmorChange.Invoke();
    }

    public event Action onHeal;
    public void Heal()
    {
        onHeal.Invoke();
    }

    public event Action onTakeDamage;
    public void TakeDamage()
    {
        onTakeDamage.Invoke();
    }
    
    public event Action onPlayerShoot;
    public void PlayerShoot()
    {
        onPlayerShoot.Invoke();
    }

    public event Action onPlayerADS;
    public void PlayerADS()
    {
        isAiming = true;
        onPlayerADS.Invoke();
    }

    public event Action onPlayerLeaveADS;
    public void PlayerLeaveADS()
    {
        isAiming = false;
        onPlayerLeaveADS.Invoke();
    }

    public event Action onPlayerReload;
    public void PlayerReload()
    {
        onPlayerReload?.Invoke();
        hasAmmo = true;
    }

    public event Action onPlayerShootGrapple;
    public void PlayerShootGrapple()
    {
        onPlayerShootGrapple?.Invoke();
    }

    public event Action onPlayerGrappleReachLocation;
    public void PlayerGrappleReachLocation()
    {
        onPlayerGrappleReachLocation?.Invoke();
    }

    public event Action onPlayerSwitchWeapons;
    public void PlayerSwitchWeapons()
    {
        onPlayerSwitchWeapons?.Invoke();
    }

    public event Action onPlayerGrappleEnemyDone;
    public void PlayerEnemyGrappleDone()
    {
        onPlayerGrappleEnemyDone?.Invoke();
    }

    public event Action onPlayerWalk;
    public void Walk()
    {
        onPlayerWalk.Invoke();
    }

    public event Action onPlayerSprint;
    public void Sprint()
    {
        onPlayerSprint.Invoke();
    }

    public event Action onPlayerCrouch;
    public void Crouch()
    {
        onPlayerCrouch.Invoke();
    }
    
    public event Action onPlayerLeaveCrouch;

    public void LeaveCrouch()
    {
        onPlayerLeaveCrouch.Invoke();
    }

    public event Action onPlayerSlide;
    public void Slide()
    {
        onPlayerSlide.Invoke();
    }


    public event Action onPlayerIdle;
    public void Idle()
    {
        onPlayerIdle.Invoke();
    }
}
