using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class Door : Interactable
{
    public Animator _animator;

    public List<KeyData> keys = new List<KeyData>();

    public bool open = false;

    public bool canOpenClose = true;

    public bool isMagnet;
    public bool keyRequired;

    public bool autoClose;

    public Action onOpen;
    public Action onClose;

    [SerializeField] private Vector3 openPos;
    [SerializeField] private Vector3 closePos;
    
    [SerializeField] private float openCloseDuration;

    private void Start()
    {
        isMagnet = GetComponent<Magnet>();
        closePos = transform.localPosition;
        keyRequired = keys.Count > 0;
    }

    public override void Interact(PlayerManager playerManager = null, bool magneticInteraction = false)
    {
        if (isMagnet && magneticInteraction)
        {
            if (!canOpenClose) return;

            if (!open)
                Open();
            else
                Close();
        }
        else
        {
            if (keyRequired && !SaveData.Current.inventory.keyRing.Contains(keys[0].keyID)) return;

            if (!canOpenClose) return;

            if (!open)
                Open();
            else
                Close();
        }
    }

    public override void OnHighlight()
    {
        string openClose = !open ? data.activateToolTip : data.deactivateToolTip;

        if (keyRequired)
        {
            data.toolTip = SaveData.Current.inventory.keyRing.Contains(keys[0].keyID)
                ? openClose
                : string.Format("Needs {0}.", keys[0].keyName);
        }
        else
        {
            data.toolTip = openClose;
        }
    }

    public void Open()
    {
        canOpenClose = false;

        gameObject.transform.DOLocalMove(openPos, openCloseDuration).OnComplete(() =>
        {
            if (autoClose)
            {
                StartCoroutine(AutoClose());
            }
            else
            {
                canOpenClose = true;
                open = true; 
            }
            
            onOpen?.Invoke();
        });
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(20f);
        
        Close();
    }

    public void Close()
    {
        canOpenClose = false;

        gameObject.transform.DOLocalMove(closePos, openCloseDuration).OnComplete(() =>
        {
            canOpenClose = true;
            open = false; 
            onClose?.Invoke();
        });
    }

    public bool IsMagnetic()
    {
        var magnet = GetComponent<Magnet>();

        return magnet != null;
    }
}