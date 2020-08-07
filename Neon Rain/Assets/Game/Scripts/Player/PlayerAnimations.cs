using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _rightHandAnimator;
    [SerializeField] private Animator _leftHandAnimator;
    
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Idle = Animator.StringToHash("Idle");

    void Start()
    {
        PlayerEvents.Current.onPlayerShoot += OnShoot;   
    }

    private void OnShoot()
    {
        _rightHandAnimator.Play(Idle);
        _rightHandAnimator.SetTrigger(Shoot);
    }
}
