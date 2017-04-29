using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabit : MonoBehaviour {

    BezierPoint bPoint;

    float delta = 0.01f;

    private void Start()
    {
        bPoint = GetComponent<BezierPoint>();
    }

    private void Update()
    {
        bPoint.CurveTime += delta;
        if (bPoint.CurveTime == 1  || bPoint.CurveTime == 0)
        {
            delta *= -1;
        }
    }

    void OnBezierEnd(BezierPoint bPt)
    {
        float scaleX = 1 - bPt.CurveTime * 2f;
        Vector3 scale = transform.localScale;
        scale.x = scaleX;
        transform.localScale = scale;
    }
}
