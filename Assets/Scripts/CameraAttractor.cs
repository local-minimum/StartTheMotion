using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttractor : MonoBehaviour {
    
    public Transform isTracked;

   
    [SerializeField]
    BezierCurve startCurve;

    BezierCurve currentCurve;
    NearCurves neighbours;

    private void Start()
    {
        currentCurve = startCurve;
        neighbours = currentCurve.GetComponent<NearCurves>();
    }

    void Update () {

        var currentCurve = this.currentCurve;
        Vector3 trackPos = isTracked.position;
        Vector3 currentBest = currentCurve.GetClosestPointOnBezier(trackPos);
        float currentSqDist = Vector3.SqrMagnitude(currentBest - trackPos);

        for (int i=0, l=neighbours.neighbours.Count; i<l; i++)
        {
            Vector3 tmp = neighbours.neighbours[i].GetClosestPointOnBezier(trackPos);
            float tmpSqDist = Vector3.SqrMagnitude(tmp - trackPos);
            if (tmpSqDist < currentSqDist)
            {
                currentCurve = neighbours.neighbours[i];
                currentBest = tmp;
                currentSqDist = tmpSqDist;
            }
        }
        if (this.currentCurve != currentCurve)
        {
            this.currentCurve = currentCurve;
            neighbours = currentCurve.GetComponent<NearCurves>();
        }
        transform.position = currentBest;

    }
}
