using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThouShallNotPass : MonoBehaviour {

    [SerializeField]
    DeathLife guardian;

    [SerializeField]
    bool passAlive;

    [SerializeField, Range(0, 1)]
    float stopPoint;

    [SerializeField]
    BezierZone stopZone;

    [SerializeField]
    BezierPoint player;

    [SerializeField]
    BezierCurve curve;

	void Update () {
        if (stopZone)
        {
            if (stopZone.IsInside(player.Curve, player.CurveTime) && guardian.alive != passAlive)
            {
                player.CurveTime = stopZone.ClosestEdgeTime(player.CurveTime);
            }
        } else if (guardian.alive != passAlive && player.Curve == curve)
        {
            if (player.CurveTime > stopPoint)
            {
                player.CurveTime = stopPoint;
            }

        }
    }
}
