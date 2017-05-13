using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThouShallNotPass : MonoBehaviour {

    [SerializeField]
    DeathLife guardian;

    [SerializeField]
    LifeDeathCondition passCondition;

    [SerializeField]
    BezierPoint stopPoint;

    [SerializeField]
    bool stopPointBelow = true;

    [SerializeField]
    BezierZone stopZone;

    [SerializeField]
    BezierPoint player;

    [SerializeField]
    BezierCurve curve;

	void Update () {
        bool stopping = !DeathLife.Compatible(passCondition, guardian.alive);
        if (stopZone)
        {
            if (stopping && stopZone.IsInside(player.Curve, player.CurveTime))
            {
                player.CurveTime = stopZone.ClosestEdgeTime(player.CurveTime);
            }
        } else if (stopping && player.Curve == curve)
        {
            float t = player.CurveTime;
            if (stopPointBelow && t > stopPoint.CurveTime)
            {
                player.CurveTime = stopPoint.CurveTime;
            } else if (!stopPointBelow && t < stopPoint.CurveTime)
            {
                player.CurveTime = stopPoint.CurveTime;
            }

        }
    }
}
