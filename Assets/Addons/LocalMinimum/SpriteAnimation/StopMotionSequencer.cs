using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SequenceMode {ForwardLoop, BackwardLoop, PingPong};

public class StopMotionSequencer : MonoBehaviour {

    SpriteRenderer sRend;

    [SerializeField]
    string sequenceName;

    public string SequenceName
    {
        get
        {
            return sequenceName;
        }
    }

    [SerializeField]
    SequenceMode mode;

    [SerializeField]
    Sprite[] sequence;

    [SerializeField]
    bool[] enabledAnimationStep;

    int showingIndex;

    int sequenceDirection = 1;

    bool m_isPlaying;

    public bool IsPlaying
    {
        get
        {
            return m_isPlaying;
        }
    }

	public void Step()
    {
        m_isPlaying = false;
        SetNextFrame();
        sRend.sprite = sequence[showingIndex];
    }

    [SerializeField] float m_fps = 15;

    private void Start()
    {
        if (Headless && Alone)
        {
            Play();
        }
    }

    public bool Headless
    {
        get
        {
            return GetComponent<StopMotionAnimator>() == null;
        }
    }

    public bool Alone
    {
        get
        {
            return GetComponents<StopMotionSequencer>().Length == 1;
        }
    }

    public void Play()
    {
        if (!m_isPlaying)
        {
            SetupRenderer();
            SyncArrayLenghts();
            SetSequenceDirection();
            StartCoroutine(Animate(m_fps));
        }
    }

    void SetSequenceDirection()
    {
        if (mode == SequenceMode.BackwardLoop)
        {
            sequenceDirection = -1;
        }
        else
        {
            sequenceDirection = 1;
        }
    }

    void SetupRenderer()
    {
        if (!sRend)
        {
            sRend = GetComponent<SpriteRenderer>();
        }
        if (!sRend)
        {
            sRend = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    void SyncArrayLenghts()
    {
        if (enabledAnimationStep.Length < sequence.Length)
        {
            bool[] tmpArr = new bool[sequence.Length];
            System.Array.Copy(enabledAnimationStep, tmpArr, enabledAnimationStep.Length);
            for (int i = enabledAnimationStep.Length; i < tmpArr.Length; i++)
            {
                tmpArr[i] = true;
            }
            enabledAnimationStep = tmpArr;
        }

    }

    public void Stop()
    {
        m_isPlaying = false;  

    }

    public void SetAnimationStepEnabled(int index)
    {
        enabledAnimationStep[index] = true;
    }

    public void SetAnimationStepDisabled(int index)
    {
        enabledAnimationStep[index] = false;
    }

    IEnumerator<WaitForSeconds> Animate(float fps)
    {
        float delta = 1f / fps;
        m_isPlaying = true;
        while (m_isPlaying)
        {
            SetNextFrame();
            sRend.sprite = sequence[showingIndex];
            yield return new WaitForSeconds(delta);
        }
    }

    void SetNextFrame()
    {
        int start = showingIndex;

        do
        {
            showingIndex+=sequenceDirection;
            if (showingIndex < 0)
            {
                if (mode == SequenceMode.PingPong)
                {
                    showingIndex = 1;
                    sequenceDirection = 1;
                }
                else
                {
                    showingIndex = sequence.Length - 1;
                }
            } else if (showingIndex >= sequence.Length)
            {
                if (mode == SequenceMode.PingPong)
                {
                    showingIndex = sequence.Length - 2;
                    sequenceDirection = -1;
                } else
                {
                    showingIndex = 0;
                }
            }

            if (showingIndex == start)
            {
                m_isPlaying = false;
                throw new System.ArgumentException("No frames enabled on " + name);
            }
        } while (!enabledAnimationStep[showingIndex]);
    }
}
