using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLine : MonoBehaviour {
    [SerializeField, HideInInspector]
    Vector3[] points = new Vector3[3];

    
    public Vector3 pt0
    {
        get
        {
            return points[0];
        }

        set
        {
            points[0] = value;
        }
    }

    public Vector3 pt1
    {
        get
        {
            return points[1];
        }

        set
        {
            points[1] = value;
        }
    }

}
