using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Elevator : Interactable
{
    public List<KeyData> keys = new List<KeyData>();

    public bool up = false;
    
    public bool canOpenClose = true;

    public bool isMagnet;
    public bool keyRequired;

    public Door bottomDoor;
    public Door topDoor;

    public GameObject platform;

    public Vector3 topPosition;
    public Vector3 bottomPosition;
    
    
    private void Start()
    {
        isMagnet = GetComponent<Magnet>();
        keyRequired = keys.Count > 0;
    }
    
    public override void Interact(PlayerManager playerManager = null, bool magneticInteraction = false)
    {
        if (isMagnet && magneticInteraction)
        {
            if (!canOpenClose) return;
            
            if (!up)
                GoUp();
            else
            {
                GoDown();
            }
        }
        else
        {
            if (keyRequired && !SaveData.Current.inventory.keyRing.Contains(keys[0].keyID)) return;
           
            if (!canOpenClose) return;

            if (!up)
                GoUp();
            else
            {
                GoDown();
            }
        }
    }

    private void GoUp()
    {
        canOpenClose = false;

        bottomDoor.Interact();
        
        platform.transform.DOLocalMove(topPosition, 5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            canOpenClose = true;
            up = true;
            topDoor.Interact();
        });
    }

    private void GoDown()
    {
        canOpenClose = false;
        
        topDoor.Interact();
        
        platform.transform.DOLocalMove(bottomPosition, 5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            canOpenClose = true;
            up = false;
            bottomDoor.Interact();
        });
    }

    public override void OnHighlight()
    {
        string openClose = !up ? data.activateToolTip : data.deactivateToolTip;

        if (keyRequired)
        {
            data.toolTip = SaveData.Current.inventory.keyRing.Contains(keys[0].keyID) ? openClose : string.Format("Needs {0}.", keys[0].keyName);
        }
        else
        {
            data.toolTip = openClose;
        }
    }
}
