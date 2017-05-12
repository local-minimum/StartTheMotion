using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour {

    [SerializeField]
    float temporalOffset = 1.5f;

    [SerializeField]
    bool stopIfNotMooving;

    [SerializeField]
    float stillSqDelta = 0.1f;

    [SerializeField]
    Transform tracking;

    [SerializeField]
    BezierZone zone;

    List<KeyValuePair<float, float>> timeToCurveTime = new List<KeyValuePair<float, float>>();
    
    int currentPlayingIndex;

    Vector3 prevPostion = Vector3.zero;
    int currentRecordingIndex;
    bool still = true;

    void Update()
    {
        UpdateTracking();
        UpdateZone();
    }

    void UpdateZone()
    {
        int l = timeToCurveTime.Count;
        float myTime = Time.timeSinceLevelLoad - temporalOffset;

        int bestPrior = -1;
        float bestPriorT = -temporalOffset;
        float bestPriorCurveT = 0;
        for (int i =0; i< l; i++)
        {
            var data = timeToCurveTime[i];
            if (data.Key < myTime && data.Key > bestPriorT)
            {
                bestPrior = i;
                bestPriorT = data.Key;
                bestPriorCurveT = data.Value;
            }
        }

        if (bestPrior < 0)
        {
            return;
        }

        int bestNext = bestPrior;
        float bestNextT = 0;
        float bestNextCurveT = 0;
        do
        {
            bestNext++;
            bestNext %= l;
            var data = timeToCurveTime[bestNext];
            if (data.Key > myTime)
            {
                bestNextT = data.Key;
                bestNextCurveT = data.Value;
                break;
            }
        } while (bestNext != bestPrior);

        
    }

    void UpdateTracking()
    {
        Vector3 trackPos = tracking.position;
        var entry = new KeyValuePair<float, float>(Time.timeSinceLevelLoad, zone.curve.TimeClosestTo(trackPos));
        if (Vector3.SqrMagnitude(prevPostion - trackPos) < stillSqDelta)
        {
            still = true;
            for (int i = 0, l = timeToCurveTime.Count; i < l; i++)
            {
                if (i != currentPlayingIndex)
                {
                    timeToCurveTime[currentRecordingIndex] = entry;
                }
                else
                {
                    timeToCurveTime[i] = new KeyValuePair<float, float>(timeToCurveTime[i].Key + Time.deltaTime, timeToCurveTime[i].Value);
                }
            }


        }
        else
        {
            still = false;
            currentRecordingIndex++;
            if (timeToCurveTime.Count <= currentRecordingIndex)
            {
                if (currentPlayingIndex < 2)
                {
                    timeToCurveTime.Add(entry);
                    return;
                }
            }
            timeToCurveTime[currentRecordingIndex] = entry;
        }
    }

    int PreviousIndex(int index)
    {
        index--;
        if (index < 0)
        {
            return index + timeToCurveTime.Count;
        }
        return index;
    }

    
}
