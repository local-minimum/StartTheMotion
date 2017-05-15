using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMotionAnimator : MonoBehaviour {

    [SerializeField]
    StopMotionSequencer[] sequences;
   
    [SerializeField]
    List<StopMotionTransition> transitions = new List<StopMotionTransition>();

    public void AddTransition(StopMotionTransition transition)
    {
        transitions.Add(transition);
    }

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

   public bool HasActiveAnimtion
   {
        get
        {
            return active != null;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return active != null && active.IsPlaying;
        }
    }

    public void PlayByName(string sequenceName, bool resetAnimation)
    {
        StopMotionSequencer next = GetSequenceByName(sequenceName);
        if (next)
        {
            Play(next, resetAnimation);
        }
        else
        {
            active = null;
            Debug.LogWarning("Requested sequence '" + sequenceName + "' but isn't known.");
        }

    }

    public void Play(StopMotionSequencer next, bool resetAnimation) { 
        if (next == active && next.IsPlaying || next == null)
        {
            Debug.LogWarning(next == null ? "Can't play null sequence" : "Next and current animations are the same '" + next.SequenceName + "'");
            return;
        }

        if (active && active != next)
        {
            active.Stop();
        }

        if (next)
        {
            next.Play(resetAnimation, OnAnimationEnd);
            active = next;
            //Debug.Log(name + ": Changed active sequence to " + active.SequenceName);
        }        
    }

    public void ActivateByName(string sequenceName)
    {
        StopMotionSequencer next = GetSequenceByName(sequenceName);
        if (next)
        {
            if (active == next)
            {
                Debug.LogWarning("Next and current animations are the same '" + next.SequenceName + "'");
            } else if (active)
            {
                active.Stop();
            }
            active = next;
        }
        else
        {
            active = null;
            Debug.LogWarning("Requested sequence '" + sequenceName + "' but isn't known.");
        }

    }

    public void StepActive()
    {
        active.Step();
    }

    public void StepToRandomInActive()
    {
        active.ShowIndex(Random.Range(0, active.Length));
    }

    public bool HasTrigger(string trigger)
    {
        for (int i = 0, l = transitions.Count; i < l; i++)
        {
            if (transitions[i].CanTrigger(this, trigger))
            {
                return true;
            }
        }
        return false;
    }

    public void Trigger(string trigger, bool reset=true)
    {
        for (int i = 0, l = transitions.Count; i < l; i++)
        {
            if (transitions[i].CanTrigger(this, trigger))
            {
                transitions[i].Execute(this, reset);
                //Debug.Log(name + " is triggering with " + trigger + " to play " + transitions[i].transitionTarget);
                return;
            }
        }

        throw new System.ArgumentException(string.Format(
            "{0}, {1}: No valid trigger target for: {2}", name, ActiveName, trigger));
    }

    private void Start()
    {
        StartAnimation();
    }

    public void Resume()
    {
        if (active && !active.IsPlaying)
        {
            active.Resume();
        }
    }

    public void Stop()
    {
        if (active)
        {
            active.Stop();
        }
    }

    public void StartAnimation(bool reset=true)
    {
        for (int i = 0, l = transitions.Count; i < l; i++)
        {
            if (transitions[i].CanExecute(this))
            {
                transitions[i].Execute(this, reset);
                Debug.Log(name + ": Start animation " + transitions[i].transitionTarget);
                return;
            }
        }
        Debug.Log(name + ": No start animation for ");
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
        for (int i=0, l=transitions.Count; i<l; i++)
        {
            if (transitions[i].AutoFires && transitions[i].CanExecute(this))
            {
                Debug.Log("Transition to " + transitions[i].transitionTarget + " with transition " + i);
                transitions[i].Execute(this, true);
                return false;
            } else
            {
                //Debug.Log("Transition Refused: " + i);
            }
        }
        return true;
    }

}
