using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Door : Interactable
{
    public Animator _animator;

    public List<KeyData> keys = new List<KeyData>();

    public bool open = false;
    
    public bool canOpenClose = true;

    public bool isMagnet;
    public bool keyRequired;

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

            StartCoroutine(!open ? Open() : Close());
        }
        else
        {
            if (keyRequired && !SaveData.Current.inventory.keyRing.Contains(keys[0].keyID)) return;
           
            if (!canOpenClose) return;

            StartCoroutine(!open ? Open() : Close());
        }

       
    }

    public override void OnHighlight()
    {
        string openClose = !open ? "Open" : "Close";

        if (keyRequired)
        {
            data.toolTip = SaveData.Current.inventory.keyRing.Contains(keys[0].keyID) ? openClose : string.Format("Needs {0}.", keys[0].keyName);
        }
        else
        {
            data.toolTip = openClose;
        }
    }

    private IEnumerator Open()
    {
        _animator.Play($"Open");
        canOpenClose = false;

        yield return new WaitForSeconds(1f);

        canOpenClose = true;
        open = true;
    }

    private IEnumerator Close()
    {
        _animator.Play($"Close");
        canOpenClose = false;

        yield return new WaitForSeconds(1f);

        canOpenClose = true;
        open = false;
    }

    public bool IsMagnetic()
    {
        var magnet = GetComponent<Magnet>();

        return magnet != null;
    }
}
