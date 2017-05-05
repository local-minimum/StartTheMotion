using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearCurves : MonoBehaviour {

    public List<BezierCurve> neighbours = new List<BezierCurve>();

    private void Reset()
    {
        neighbours.Clear();
        if (transform.parent)
        {
            foreach (BezierCurve curve in transform.parent.GetComponents<BezierCurve>())
            {
                neighbours.Add(curve);
            }
        }
        foreach (Transform child in transform)
        {
            foreach (BezierCurve curve in child.GetComponents<BezierCurve>())
            {
                neighbours.Add(curve);
            }
        }
    }
}
