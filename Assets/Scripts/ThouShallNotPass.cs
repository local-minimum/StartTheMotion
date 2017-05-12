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
    BezierPoint player;

    [SerializeField]
    BezierCurve curve;

	void Update () {
        if (guardian.alive != passAlive && player.Curve == curve && player.CurveTime > stopPoint)
        {
            player.CurveTime = stopPoint;
        }	
	}
}
