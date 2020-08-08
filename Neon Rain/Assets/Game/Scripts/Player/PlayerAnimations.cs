using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _rightHandAnimator;
    [SerializeField] private Animator _leftHandAnimator;
    
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Speed = Animator.StringToHash("Speed"); //Blend tree value = Player Movement Speed

    private FirstPersonController _controller;

    void Start()
    {
        PlayerEvents.Current.onPlayerShoot += OnShoot;
        _controller = GetComponent<FirstPersonController>();
    }

    private void LateUpdate()
    {
        if (!_controller.movementInputData.JumpClicked)
            OnMove(_controller.MCurrentSpeed/4f);
    }

    private void OnShoot()
    {
        _rightHandAnimator.Play(Idle);
        _rightHandAnimator.SetTrigger(Shoot);
    }

    private void OnMove(float currentSpeed)
    {
        _rightHandAnimator.SetFloat(Speed, currentSpeed);
    }
}
