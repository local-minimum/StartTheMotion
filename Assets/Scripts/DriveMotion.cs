using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MotionParameters
{
    public bool active;    
    public AnimationCurve velocity;
    public float duration;
}

public class Driver
{
    public BezierPoint point;
    MotionParameters speedByPosition;
    MotionParameters speedByDuration;

    public void Update()
    {
        float distance = 0;
        if (speedByDuration.active)
        {
            distance += speedByDuration.velocity.Evaluate(speedByDuration.duration) * Time.deltaTime;
            speedByDuration.duration += Time.deltaTime;
        }

        if (speedByPosition.active)
        {
            distance += speedByPosition.velocity.Evaluate(point.CurveTime) * Time.deltaTime;
            speedByPosition.duration += Time.deltaTime;
        }

        point.Move(distance);
    }
}

public class DriveMotion : MonoBehaviour {


    void OnBezierEnter()
    {

    }

    void OnBezierExit()
    {

    }

    bool IsDrivingPoint(BezierPoint point)
    {
        for (int i = 0, l = automatons.Count; i < l; i++)
        {
            if (automatons[i].point == point)
            {
                return true;
            }
        }

        return false;
    }

    List<Driver> automatons = new List<Driver>();


    [SerializeField]
    MotionParameters speedByPostion;

    [SerializeField]
    MotionParameters speedByDuration;


    private void Update()
    {
        for (int i=0, l=automatons.Count; i<l; i++)
        {
            automatons[i].Update();
        }
    }
}
