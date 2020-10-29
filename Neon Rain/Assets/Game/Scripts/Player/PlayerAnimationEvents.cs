using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    void StopAttacking()
    {
        PlayerEvents.Current.isAttacking = false;
    }

    void StopSwappingWeapons()
    {
        PlayerEvents.Current.isSwappingWeapons = false;
    }

    void SetSwapped()
    {
        PlayerEvents.Current.weaponSwapped = true;
    }
}
