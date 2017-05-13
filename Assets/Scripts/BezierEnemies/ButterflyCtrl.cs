using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyCtrl : MonoBehaviour {

    DeathLife deathLife;

    [SerializeField]
    BezierCurve upCurve;

    [SerializeField]
    BezierCurve downCurve;

    BezierPoint pt;

    bool lifeState;

    void Start () {
        deathLife = GetComponent<DeathLife>();
        lifeState = deathLife.alive;
        pt = deathLife.GetComponent<BezierPoint>();
    }

    void Update () {
        if (lifeState != deathLife.alive)
        {
            lifeState = deathLife.alive;

            if (lifeState)
            {
                pt.Attach(upCurve, 1 - pt.CurveTime);
            } else
            {
                pt.Attach(downCurve, 1 - pt.CurveTime);
            }
            
        }
	}
}
