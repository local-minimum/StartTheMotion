using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BifPointInZone : MonoBehaviour {

    [SerializeField]
    Bifurcation bifurcation;

    [SerializeField]
    BezierZone zone;

    [SerializeField]
    BezierPoint point;

    [SerializeField]
    DeathLife lifeDeath;

    [SerializeField]
    bool requireCarry;

	// Use this for initialization
	void Start () {
        bifurcation.externalCondtion = AllowFork;
	}

    bool AllowFork()
    {
        if (zone.curve == point.Curve && zone.IsInside(point.Curve, point.CurveTime))
        {
            return lifeDeath == null || lifeDeath.CanCarry();
        }

        return false;
    }
}
