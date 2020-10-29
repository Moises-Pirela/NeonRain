using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol
{
    Waypoint _currentWaypoint { get; set; }

    List<Waypoint> _patrolPoints { get; set; }
}
