using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VHS;

public class PlayerAnimations : MonoBehaviour
{
    [FormerlySerializedAs("_rightHandAnimator")] [SerializeField] private Animator _handsAnimator;
    
    [SerializeField] private Animator rightHandController;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Speed = Animator.StringToHash("Speed"); //Blend tree value = Player Movement Speed

    [SerializeField] private FirstPersonController _controller;
    private static readonly int Swap = Animator.StringToHash("Swap");

    void Start()
    {
        PlayerEvents.Current.onPlayerShoot += OnShoot;
        PlayerEvents.Current.onPlayerMelee += OnMelee;
        PlayerEvents.Current.onSwapWeapon += OnWeaponSwap;
    }

    private void LateUpdate()
    {
        if (!_controller.movementInputData.JumpClicked && !_controller.movementInputData.Dashed)
            OnMove(_controller.MCurrentSpeed/6f);
    }

    private void OnShoot()
    {
        rightHandController.Play(Idle);
        PlayerEvents.Current.isAttacking = true;
        rightHandController.SetTrigger(Attack);
    }

    private void OnMelee()
    {
        rightHandController.Play(Idle);
        PlayerEvents.Current.isAttacking = true;
        rightHandController.SetTrigger(Attack);
    }

    private void OnWeaponSwap()
    {
        PlayerEvents.Current.isSwappingWeapons = true;
        PlayerEvents.Current.weaponSwapped = false;
        _handsAnimator.SetTrigger(Swap);
    }

    private void OnMove(float currentSpeed)
    {
        _handsAnimator.SetFloat(Speed, currentSpeed);
    }
}
