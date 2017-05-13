using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFollowTransform : MonoBehaviour {

    [SerializeField]
    Transform guide;

    [SerializeField]
    BezierPoint pt;

    private void Awake()
    {
        //Debug.Log(pt.Curve.GetComponentPointGlobal(3));

    }

    void Update () {
        pt.CurveTime = pt.Curve.TimeClosestTo(guide.position);
        //Debug.Log(pt.Curve.GetComponentPointGlobal(3));
	}
}
