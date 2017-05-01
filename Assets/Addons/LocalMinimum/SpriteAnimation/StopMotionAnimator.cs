using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStopMotionTransition
{
    string transitionSource { get; }
    string transitionTarget { get; }
    bool AutoFires { get; }
    bool CanExecute(StopMotionAnimator animator);
    bool CanTrigger(StopMotionAnimator animator, string trigger);
    void Execute(StopMotionAnimator animator);
    
}

public class StopMotionAnimator : MonoBehaviour {

    [SerializeField, HideInInspector]
    StopMotionSequencer[] sequences;

    [SerializeField, HideInInspector]
    IStopMotionTransition[] transitions;

    private void Reset()
    {
        sequences = GetComponents<StopMotionSequencer>();
    }

    StopMotionSequencer active;

    public string ActiveName
    {
        get
        {
            if (active)
            {
                return active.SequenceName;
            } else
            {
                return null;
            }
        }
    }

    public void PlayByName(string sequenceName)
    {
        StopMotionSequencer next = GetSequenceByName(sequenceName);
        if (next)
        {
            Play(next);
        }
        else
        {
            active = null;
            Debug.LogWarning("Requested sequence '" + sequenceName + "' but isn't known.");
        }

    }

    public void Play(StopMotionSequencer next) { 
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
            next.Play(OnAnimationEnd);
            active = next;
        }        
    }

    public void Trigger(string trigger)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].transitionSource == active.SequenceName && transitions[i].CanTrigger(this, trigger))
            {
                transitions[i].Execute(this);
                return;
            }
        }

        throw new System.ArgumentException("No valid trigger target for: " + trigger);
    }

    public void StartAnimation()
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].transitionSource == active.SequenceName && transitions[i].CanExecute(this))
            {
                transitions[i].Execute(this);
            }
        }
    }

    void Ease(StopMotionSequencer a, StopMotionSequencer b)
    {
        throw new System.NotImplementedException();
    }

    public StopMotionSequencer GetSequenceByName(string name)
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

    private bool OnAnimationEnd()
    {
        for (int i=0; i<transitions.Length; i++)
        {
            if (transitions[i].AutoFires && transitions[i].CanExecute(this))
            {
                transitions[i].Execute(this);
                return false;
            }
        }
        return true;
    }

}
