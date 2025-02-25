﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            Debug.Log("Renderer is empty");
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;
    }
}
