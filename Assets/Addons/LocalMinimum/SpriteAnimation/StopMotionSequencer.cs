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
        SetupRenderer();
        SyncArrayLenghts();
        SetSequenceDirection();
        if (SetNextFrame())
        {
            sRend.sprite = sequence[showingIndex];
        }
    }

    [SerializeField] float m_fps = 15;

    StopMotionAnimator smAnimator;

    private void Start()
    {
        smAnimator = GetComponent<StopMotionAnimator>();
        if (Headless && Alone)
        {
            Play();
        }
    }

    public bool Headless
    {
        get
        {
            return smAnimator == null;
        }
    }

    public bool Alone
    {
        get
        {
            return GetComponents<StopMotionSequencer>().Length == 1;
        }
    }

    System.Func<bool> callbackOnEndPlayback;

    public void Play(System.Func<bool> callbackOnEndPlayback)
    {
        if (!m_isPlaying)
        {
            this.callbackOnEndPlayback = callbackOnEndPlayback;
            SetupRenderer();
            SyncArrayLenghts();
            SetSequenceDirection();
            StartCoroutine(Animate(m_fps));
        }else
        {
            Debug.LogWarning(name + " sequencer is already running");
        }
    }

    public void Play()
    {
        if (!m_isPlaying)
        {
            callbackOnEndPlayback = null;
            SetupRenderer();
            SyncArrayLenghts();
            SetSequenceDirection();
            StartCoroutine(Animate(m_fps));
        }
        else
        {
            Debug.LogWarning(name + " sequencer is already running");
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

    float m_lastUpdate;

    public float LastUpdate
    {
        get
        {
            return m_lastUpdate;
        }
    }

    IEnumerator<WaitForSeconds> Animate(float fps)
    {
        float delta = 1f / fps;
        m_isPlaying = true;
        while (m_isPlaying)
        {
            if (SetNextFrame())
            {
                sRend.sprite = sequence[showingIndex];
                m_lastUpdate = Time.timeSinceLevelLoad; 
                yield return new WaitForSeconds(delta);
            } else
            {
                break;
            }
        }
    }

    bool SetNextFrame()
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
                    if (callbackOnEndPlayback != null)
                    {
                        if (!callbackOnEndPlayback())
                        {
                            return false;
                        }
                    }
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
                    if (callbackOnEndPlayback != null)
                    {
                        if (!callbackOnEndPlayback())
                        {
                            return false;
                        }
                    }
                }
            }

            if (showingIndex == start)
            {
                m_isPlaying = false;
                throw new System.ArgumentException("No frames enabled on " + name);
            }
        } while (!enabledAnimationStep[showingIndex]);

        return true;
    }
}
