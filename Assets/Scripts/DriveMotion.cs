using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MotionParameters
{
    public bool active;    
    public AnimationCurve velocity;
    public float duration;

    public MotionParameters Clone()
    {
        var clone = new MotionParameters();
        clone.active = active;
        clone.duration = duration;
        clone.velocity = velocity;
        return clone;
    }
}

public class Driver
{
    public BezierPoint point;
    public MotionParameters speedByPosition;
    public MotionParameters speedByDuration;

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
            //Debug.Log(point.CurveTime);
            //Debug.Log(speedByPosition.velocity.Evaluate(point.CurveTime));

            distance += speedByPosition.velocity.Evaluate(point.CurveTime) * Time.deltaTime;
            speedByPosition.duration += Time.deltaTime;
        }

        //Debug.Log(distance);
        point.Move(distance);
    }
}

public class DriveMotion : MonoBehaviour {


    void OnAttachToCurve(BezierPoint point)
    {
        if (!IsDrivingPoint(point))
        {
            AddDriver(point);
            point.SendMessage("OnPointDriven", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDetachFromCurve(BezierPoint point)
    {
        if (IsDrivingPoint(point))
        {
            RemoveDriver(point);
            point.SendMessage("OnPointNotDriven", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void AddDriver(BezierPoint point)
    {
        var driver = new Driver();
        driver.point = point;
        driver.speedByDuration = speedByDuration.Clone();
        driver.speedByPosition = speedByPostion.Clone();
        automatons.Add(driver);
    }

    void RemoveDriver(BezierPoint point)
    {
        for (int i=0, l=automatons.Count; i<l; i++)
        {
            if (automatons[i].point == point)
            {
                automatons.RemoveAt(i);
            }
        }
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

    public string[] canBeDriven;

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
