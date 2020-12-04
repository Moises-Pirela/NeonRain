using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light light;

    private float flickerTime = 1f;
    private float flickerTimer;
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= flickerTime)
        {
            light.enabled = !light.enabled;
            flickerTimer = 0;
        }
    }
}
