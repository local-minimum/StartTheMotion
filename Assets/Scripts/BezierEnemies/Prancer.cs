using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prancer : MonoBehaviour {

    BezierPoint bPoint;

    [SerializeField]
    float baseSpeed = 1f;

    float sign = 1;
    [SerializeField]
    AnimationCurve speed;

    private void Start()
    {
        bPoint = GetComponent<BezierPoint>();
    }

    private void Update()
    {
        bPoint.CurveTime += speed.Evaluate(bPoint.CurveTime) * Time.deltaTime * sign * baseSpeed;
        if (bPoint.CurveTime == 1  || bPoint.CurveTime == 0)
        {
            sign *= -1;
        }
    }

    [SerializeField]
    bool turnOnCurveEnd;

    void OnBezierEnd(BezierPoint bPt)
    {
        if (turnOnCurveEnd)
        {
            float scaleX = 1 - bPt.CurveTime * 2f;
            Vector3 scale = transform.localScale;
            scale.x = scaleX;
            transform.localScale = scale;
        }
    }

    void OnBezierZoneEnter(BezierZone zone)
    {
        Debug.Log(zone);
    }
}
