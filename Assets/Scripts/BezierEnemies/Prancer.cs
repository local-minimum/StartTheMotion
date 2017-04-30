using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prancer : MonoBehaviour {

    BezierPoint bPoint;

    [SerializeField]
    float baseSpeed = 1f;

    float walkDirection = 1;
    [SerializeField]
    AnimationCurve speed;

    private void Start()
    {
        bPoint = GetComponent<BezierPoint>();
    }

    private void Update()
    {
               
        bPoint.Move(speed.Evaluate(bPoint.CurveTime) * Time.deltaTime * walkDirection * baseSpeed);
        
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

        walkDirection *= -1;
    }

    void OnBezierZoneEnter(BezierZone zone)
    {
        Debug.Log(zone);
    }
}
