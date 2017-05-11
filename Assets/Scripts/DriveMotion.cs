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

    BezierCurve curve;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (driveForZoneOnly)
        {
            if (bEvent.type == BezierZoneEventType.ExitZone)
            {
                if (IsDrivingPoint(bEvent.point))
                {
                    RemoveDriver(bEvent.point);
                }
            }
            else if (!IsDrivingPoint(bEvent.point) && CanDrivePoint(bEvent.point))
            {
                AddDriver(bEvent.point);
            }
        }
    }

    void OnAttachToCurve(BezierPoint point)
    {
        if (!IsDrivingPoint(point) && CanDrivePoint(point) && !driveForZoneOnly)
        {
            AddDriver(point);
            point.SendMessage("OnPointDriven", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDetachFromCurve(BezierPoint point)
    {
        Debug.Log(name + ": Remove driver " + point + "? " + IsDrivingPoint(point));
        if (IsDrivingPoint(point))
        {
            RemoveDriver(point);
            point.SendMessage("OnPointNotDriven", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void AddDriver(BezierPoint point)
    {
        Debug.Log(name + ": Driving " + point);
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
                Debug.Log(name + ": Not Driving " + automatons[i].point);
                automatons.RemoveAt(i);
            }
        }

        Debug.Log(name + ": Drivers remaining " + automatons.Count);
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

    bool CanDrivePoint(BezierPoint point)
    {
        for (int i=0;i<canBeDriven.Length; i++)
        {
            if (point.CompareTag(canBeDriven[i]))
            {
                return true;
            }
        }
        return false;
    }
    
    [SerializeField]
    string[] canBeDriven;

    [SerializeField]
    bool driveForZoneOnly = false;

    List<Driver> automatons = new List<Driver>();


    [SerializeField]
    MotionParameters speedByPostion;

    [SerializeField]
    MotionParameters speedByDuration;

    void Start()
    {
        curve = GetComponent<BezierCurve>();
    }

    private void Update()
    {
        for (int i=0, l=automatons.Count; i<l; i++)
        {
            if (automatons[i].point.Curve == curve)
            {
                automatons[i].Update();
            } else
            {
                Debug.LogWarning(string.Format("{0}: Driver #{1}, {2} should have been removed because on other curve", name, i, automatons[i]));
            }
        }
    }
}
