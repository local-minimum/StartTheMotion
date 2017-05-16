using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stutter : MonoBehaviour {

    [SerializeField]
    int stutterLengthMin = -1;

    [SerializeField]
    int stutterLengthMax = 7;

    StopMotionAnimator stAnim;
   
    List<List<string>> stutterGroup = new List<List<string>>();

    List<int> stutters = new List<int>();

    public void AddStutter(List<string> sequencerNames, int nStutters)
    {
        stutterGroup.Add(sequencerNames);
        stutters.Add(nStutters);
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
            if (stutterGroup[i].Contains(seq.SequenceName))
            {
                seq.FastForward(Random.Range(stutterLengthMin, stutterLengthMax));
                stutters[i]--;
                if (stutters[i] < 1)
                {
                    stutters.RemoveAt(i);
                    stutterGroup.RemoveAt(i);
                }
                break;
            }
        }
    }
}
