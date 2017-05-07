using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PositionInstruction
{
    public string name
    {
        get
        {
            return string.Format("{0},{1}:{2}", sequencerGameObject, sequencerName, sequenceImageName);
        }
    }

    public string sequenceImageName;
    public string sequencerName;
    public string sequencerGameObject;
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

    public PositionInstruction Clone()
    {
        var positionInstruction = new PositionInstruction();
        positionInstruction.sequenceImageName = sequenceImageName;
        positionInstruction.position = position;
        positionInstruction.sequencerGameObject = sequencerGameObject;
        positionInstruction.sequencerName = sequencerName;
        return positionInstruction;
    }
}

[ExecuteInEditMode]
public class PositionWithAnimation : MonoBehaviour {

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

    int insertIndex;

    private void PositionWithAnimation_OnSequenceFrame(StopMotionSequencer sequencer)
    {
        currentSequencer = sequencer;
        for (int i = 0, l = positions.Count; i < l; i++)
        {
            if (positions[i].HasSameNames(sequencer))
            {
                editingPosition = positions[i].Clone();
                LoadPosition();
                insertIndex = i;
                break;
            }
        }
        editingPosition = new PositionInstruction(sequencer, this);
        insertIndex = -1;
    }

    public void RecordPosition()
    {
        if (simulationSpace == Space.Self)
        {
            editingPosition.position = transform.localPosition;
        }
        else
        {
            editingPosition.position = transform.position - currentSequencer.transform.position;
        }

        if (insertIndex < 0)
        {
            insertIndex = positions.Count;
            positions.Add(editingPosition.Clone());
        } else
        {
            positions[insertIndex] = editingPosition.Clone();
        }
    }

    public void LoadPosition()
    {
        if (simulationSpace == Space.Self)
        {
            transform.localPosition = editingPosition.position;
        } else
        {
            transform.position = currentSequencer.transform.TransformVector(editingPosition.position) + currentSequencer.transform.position;

        }
    }

    public void StepCurrentAnimation()
    {
        currentSequencer.Step();
    }
}
