using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierZone : MonoBehaviour {

    public float[] times = new float[2] { 0, 1 };
    public float left
    {
        get {
            return times[0];
        }

    }

    public float right
    {
        get
        {
            return times[1];
        }
    }

    public BezierCurve curve;

#if UNITY_EDITOR

    public Color lineColor;

#endif
}
