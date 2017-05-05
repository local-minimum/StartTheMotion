using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PositionInstruction
{
    public string sequencerName;
    public string sequencerGameObject;
    public string sequenceImageName;
    public Vector3 position;
    
    public PositionInstruction(StopMotionSequencer sequencer, PositionWithAnimation posWithAnim)
    {
        sequencerGameObject = sequencer.name;
        sequencerName = sequencer.SequenceName;
        sequenceImageName = sequencer.ShowingImageName;

        if (posWithAnim.SimulationSpace == Space.Self)
        {
            position = posWithAnim.transform.position;
        } else
        {
            position = posWithAnim.transform.localPosition;
        }
    }

    public bool HasSameNames(StopMotionSequencer other)
    {
        return sequenceImageName == other.ShowingImageName && sequencerName == other.SequenceName && sequencerGameObject == other.name;
    }
}

[ExecuteInEditMode]
public class PositionWithAnimation : MonoBehaviour {

    PositionInstruction nullPosition = new PositionInstruction();

    [SerializeField]
    Space simulationSpace = Space.Self;

    public Space SimulationSpace
    {
        get
        {
            return simulationSpace;
        }
    }

    [SerializeField]
    StopMotionSequencer[] trackingSequences;

    private void OnEnable()
    {
        if (trackingSequences == null)
        {
            return;
        }
        for (int i=0; i<trackingSequences.Length; i++)
        {
            trackingSequences[i].OnSequenceFrame += PositionWithAnimation_OnSequenceFrame;
        }
    }

    private void OnDisable()
    {
        if (trackingSequences == null)
        {
            return;
        }
        for (int i=0; i<trackingSequences.Length; i++)
        {
            trackingSequences[i].OnSequenceFrame -= PositionWithAnimation_OnSequenceFrame;
        }
    }

    [SerializeField]
    List<PositionInstruction> positions = new List<PositionInstruction>();

    [SerializeField]
    StopMotionSequencer currentSequencer;
    
    [SerializeField]
    PositionInstruction editingPosition;

    private void PositionWithAnimation_OnSequenceFrame(StopMotionSequencer sequencer)
    {
        currentSequencer = sequencer;
        for (int i = 0, l = positions.Count; i < l; i++)
        {
            if (positions[i].HasSameNames(sequencer))
            {
                LoadPosition(i);
                editingPosition = positions[i];
            }
            break;
        }
        editingPosition = nullPosition;
    }

    public void LoadPosition(int index)
    {
        if (simulationSpace == Space.Self)
        {
            transform.localPosition = editingPosition.position;
        } else
        {
            transform.position = editingPosition.position;
        }
    }

    public void StepCurrentAnimation()
    {

    }
}
