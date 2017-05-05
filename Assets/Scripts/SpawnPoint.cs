using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public static SpawnPoint spawnPoint;
    public static BezierZoneEvent zoneEvent;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (spawnPoint == this)
        {
            return;
        }
        if (bEvent.point.GetComponent<CharacterCtrlr>())
        {
            spawnPoint = this;
            zoneEvent = bEvent;
            Debug.Log(name + ": Set spawn postion for " + bEvent.point);
        }
    }

    private void OnDisable()
    {
        if (spawnPoint == this)
        {
            spawnPoint = null;
            zoneEvent = new BezierZoneEvent();
        }
    }

    private void OnDestroy()
    {
        if (spawnPoint == this)
        {
            spawnPoint = null;
            zoneEvent = new BezierZoneEvent();
        }
    }
}
