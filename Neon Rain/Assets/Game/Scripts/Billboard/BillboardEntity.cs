using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEntity : MonoBehaviour
{
    [SerializeField] private BillboardData _billboardData;
    [SerializeField] private BillboardEntityData _billboardEntityData;

    private int angleIndex;

    [SerializeField] private SpriteRenderer spriteObject;

    private void LateUpdate()
    {
        CalculateIndex();
    }

    public void CalculateIndex()
    {
        var forward = transform.forward;
        var direction = AIBlackboard._current.GetPlayerPosition() - transform.position;
        
        var myAngle = 360 - Vector3.Angle(direction, forward); //Compensate for 180 degrees limit

        angleIndex = 0;
        
        spriteObject.transform.localEulerAngles = new Vector3(0,myAngle + 360,0);

        if (!_billboardEntityData.isDynamic) return;
        
        for (int i = 0 ; i  < _billboardData.billboardAngles.Length; i++)
        {
            if (i < _billboardData.billboardAngles.Length - 1)
            {
                if (myAngle <= _billboardData.billboardAngles[i] && myAngle >= _billboardData.billboardAngles[i + 1])
                {
                    if (myAngle >= _billboardData.billboardAngles[i] - 22)
                        angleIndex = i;
                    else if (myAngle <= _billboardData.billboardAngles[i + 1] + 22)
                        angleIndex = i + 1;
                    
                    break;
                }
            }
            else
            {
                angleIndex = _billboardData.billboardAngles.Length - 1;
            }
        }

        spriteObject.color = _billboardEntityData.debugAnglesCycle[angleIndex];


    }

    public void LookAtCamera()
    {
            
    }
    
}
