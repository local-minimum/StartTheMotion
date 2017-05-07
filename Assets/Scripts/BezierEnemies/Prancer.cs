using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prancer : MonoBehaviour {

    BezierPoint bPoint;

    [SerializeField]
    float baseSpeed = 1f;
    
    public float walkDirection = 1;

    [SerializeField]
    AnimationCurve speed;

    Vector3 localScale;

    private void Start()
    {
        bPoint = GetComponent<BezierPoint>();
        localScale = transform.localScale;
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
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        walkDirection *= -1;
    }

    void OnBezierZoneEnter(BezierZone zone)
    {
        Debug.Log(zone);
    }
}
