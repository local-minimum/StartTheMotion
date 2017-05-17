using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stutter : MonoBehaviour {

    [SerializeField]
    int stutterLengthMin = -1;

    [SerializeField]
    int stutterLengthMax = 7;

    [SerializeField, Range(0, 1)]
    float baseStutterProb = 1;

    StopMotionAnimator stAnim;
   
    List<List<string>> stutterGroup = new List<List<string>>();

    List<int> stutters = new List<int>();

    List<float> stutterProbabilities = new List<float>();

    public void AddStutter(List<string> sequencerNames, int nStutters)
    {
        stutterGroup.Add(sequencerNames);
        stutters.Add(nStutters);
        stutterProbabilities.Add(baseStutterProb);
    }

    public void AddStutter(List<string> sequencerNames, int nStutters, float stutterProbability)
    {
        stutterGroup.Add(sequencerNames);
        stutters.Add(nStutters);
        stutterProbabilities.Add(stutterProbability);
    }

    private void Awake()
    {
        stAnim = GetComponent<StopMotionAnimator>();
    }

    private void OnEnable()
    {
        stAnim.OnAnimationEvent += StAnim_OnAnimationEvent;
    }

    private void OnDisable()
    {
        stAnim.OnAnimationEvent -= StAnim_OnAnimationEvent;
    }
   
    private void StAnim_OnAnimationEvent(StopMotionSequencer seq, AnimEventType eventType)
    {
        if (eventType != AnimEventType.NewFrame)
        {
            return;
        }

        for (int i=0, l=stutterGroup.Count; i<l; i++)
        {
            if (stutterGroup[i].Contains(seq.SequenceName) && Random.value <= stutterProbabilities[i])
            {
                seq.FastForward(Random.Range(stutterLengthMin, stutterLengthMax));
                stutters[i]--;
                if (stutters[i] < 1)
                {
                    stutters.RemoveAt(i);
                    stutterGroup.RemoveAt(i);
                    stutterProbabilities.RemoveAt(i);
                }
                break;
            }
        }
    }
}
