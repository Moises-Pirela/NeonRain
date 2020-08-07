using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    void StopShooting()
    {
        PlayerEvents.Current.canShoot = false;
    }
}
