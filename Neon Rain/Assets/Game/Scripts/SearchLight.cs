using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SearchLight : MonoBehaviour
{
    private float timer;
    [SerializeField] private float timeToTurn;

    [SerializeField]private Vector3 rotateTo;
    [SerializeField]private Vector3 rotateTo2;

    private void Start()
    {
        RotateAnimation(rotateTo, rotateTo2);
    }

    private void RotateAnimation(Vector3 rotate1, Vector3 rotate2)
    {
        transform.DOLocalRotate(rotate1, timeToTurn).OnComplete(() =>
        {
            transform.DOLocalRotate(rotate2, timeToTurn).OnComplete(() => RotateAnimation(rotate1, rotate2));
        });
    }
}
