using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagnetType {Entity, Inanimate}
public class Magnet : MonoBehaviour
{
    public bool tagged;

    public MagnetType magnetType;
}
