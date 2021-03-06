﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnFrame : MonoBehaviour {

    [SerializeField]
    BezierPoint pt;

    [SerializeField]
    StopMotionSequencer sequencer;

    [SerializeField]
    int[] moveIndices;

    [SerializeField]
    float[] moveLengths;

    [Range(-1, 1)]
    public float direction = 1;

    private void OnEnable()
    {
        sequencer.OnSequenceFrame += Sequencer_OnSequenceFrame;
    }

    private void OnDisable()
    {
        sequencer.OnSequenceFrame -= Sequencer_OnSequenceFrame;
    }

    private void Sequencer_OnSequenceFrame(StopMotionSequencer sequencer)
    {
        float epsilon = 0.01f;

        int sIdx = sequencer.ShowingIndex;
        for (int i=0; i<moveIndices.Length; i++)
        {
            if (sIdx == moveIndices[i])
            {
                float move = moveLengths[i];
                float diff = pt.Move(move * direction);
                if (diff > epsilon)
                {
                    pt.Move(diff * direction);
                }
                return;
            }
        }
        
    }
}
