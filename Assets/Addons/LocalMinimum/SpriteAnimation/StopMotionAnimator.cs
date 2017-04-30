using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InstructionType { OnLoad, OnAnimationLoop};


public class StopMotionAnimator : MonoBehaviour {

    [SerializeField]
    StopMotionSequencer[] sequences;

    private void Reset()
    {
        sequences = GetComponents<StopMotionSequencer>();
    }

    StopMotionSequencer active;

    public void Trigger(string trigger)
    {
        StopMotionSequencer next = GetSequenceByName(name);

        if (next == active && next != null)
        {
            return;
        }

        if (active)
        {
            active.Stop();
        }

        if (next)
        {
            Ease(active, next);
            active = next;
        } else
        {
            active = null;
            Debug.LogWarning("Requested sequence '" + trigger + "' but isn't known.");
        }
        
    }

    void Ease(StopMotionSequencer a, StopMotionSequencer b)
    {
        b.Play();
    }

    StopMotionSequencer GetSequenceByName(string name)
    {
        for (int i=0; i<sequences.Length; i++)
        {
            if (sequences[i].SequenceName == name)
            {
                return sequences[i];
            }
        }

        return null;
    }
}
