﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SequenceMode {ForwardLoop, BackwardLoop, PingPong};
public delegate void SequenceFrame(StopMotionSequencer sequencer);

public class StopMotionSequencer : MonoBehaviour {

    public event SequenceFrame OnSequenceFrame;

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

    [SerializeField]
    int showingIndex;

    public int ShowingIndex
    {
        get
        {
            return showingIndex;
        }
    }

    public string ShowingImageName
    {
        get
        {
            return sequence[showingIndex].name;
        }
    }

    public int Length
    {
        get
        {
            return sequence.Length;
        }
    }

    [SerializeField, Range(-1, 1)]
    int sequenceDirection = 1;

    public bool IsPlaying
    {
        get
        {
            return _activeAnimation != null;
        }

        private set
        {
            if (value || _activeAnimation == null)
            {
                Debug.LogError(name + ", " + sequenceName + " has unexpected missing animation" + 
                    string.Format("Request {0} while animation {1}", value, _activeAnimation));
            } else
            {
                StopCoroutine(_activeAnimation);
                _activeAnimation = null;
            }
        }
    }

    public void FastForward(int delta)
    {
        showingIndex += delta * sequenceDirection;

    }

    public void ShowIndex(int index)
    {
        if (IsPlaying)
        {
            IsPlaying = false;
        }
        SetupRenderer();
        SyncArrayLenghts();
        SetSequenceDirection();
        showingIndex = index;

        sRend.sprite = sequence[showingIndex];
        if (OnSequenceFrame != null)
        {
            OnSequenceFrame(this);
        }
        //Debug.Log(string.Format("{0}, {1}: Stepping to '{2}'", name, sequenceName, ShowingImageName));
    }

    public void Step()
    {
        if (IsPlaying)
        {
            IsPlaying = false;
        }
        SetupRenderer();
        SyncArrayLenghts();
        SetSequenceDirection();
        if (SetNextFrame())
        {
            sRend.sprite = sequence[showingIndex];
            if (OnSequenceFrame != null)
            {
                OnSequenceFrame(this);
            }
            Debug.Log(string.Format("{0}, {1}: Stepping to '{2}'", name, sequenceName, ShowingImageName));
        }
    }

    [SerializeField] float m_fps = 15;

    StopMotionAnimator smAnimator;

    private void Start()
    {
        smAnimator = GetComponent<StopMotionAnimator>();
        if (Headless && Alone)
        {
            Play(true);
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
    public void Resume()
    {
        if (callbackOnEndPlayback != null)
        {
            Play(false, callbackOnEndPlayback);
        } else
        {
            Play(false);
        }
    }

    public void Play(bool resetSequence, System.Func<bool> callbackOnEndPlayback)
    {
        if (!IsPlaying)
        {
            this.callbackOnEndPlayback = callbackOnEndPlayback;
            SetupRenderer();
            SyncArrayLenghts();
            SetSequenceDirection();
            if (resetSequence)
            {
                SetSequenceStart();
            }
            _activeAnimation = Animate(m_fps);
            StartCoroutine(_activeAnimation);
        } else
        {
            Debug.LogWarning(name + " sequencer " + sequenceName + " is already running");
        }
    }

    public void Play(bool resetSequence)
    {
        if (!IsPlaying)
        {
            callbackOnEndPlayback = null;
            SetupRenderer();
            if (resetSequence)
            {
                SetSequenceStart();
            }
            SyncArrayLenghts();
            SetSequenceDirection();
            _activeAnimation = Animate(m_fps);
            StartCoroutine(_activeAnimation);
        }
        else
        {
            Debug.LogWarning(name + " sequencer is already running");
        }
    }

    IEnumerator<WaitForSeconds> _activeAnimation = null;

    void SetSequenceStart()
    {
        if (mode == SequenceMode.BackwardLoop)
        {
            showingIndex = sequence.Length;
        } else if (mode == SequenceMode.ForwardLoop)
        {
            showingIndex = -1;
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
        if (IsPlaying)
        {
            IsPlaying = false;
        }
        //Debug.Log(name + ": Stopped playing " + sequenceName);
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
        
        while (true)
        {
            if (SetNextFrame())
            {
                sRend.sprite = sequence[showingIndex];
                m_lastUpdate = Time.timeSinceLevelLoad;
                if (OnSequenceFrame != null)
                {
                    OnSequenceFrame(this);
                } 
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
        if (sequence.Length == 1)
        {
            showingIndex = 0;
            return true;
        }

        int tries = 0;
        int max_tries = 10;

        do
        {
            showingIndex+=sequenceDirection;
            if (showingIndex < 0)
            {
                if (mode == SequenceMode.PingPong)
                {
                    showingIndex = -showingIndex;
                    sequenceDirection = 1;
                }
                else
                {
                    showingIndex = sequence.Length + showingIndex;
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
                    showingIndex = sequence.Length - (sequence.Length - showingIndex - 2);
                    sequenceDirection = -1;
                } else
                {
                    showingIndex = showingIndex - sequence.Length;
                    if (callbackOnEndPlayback != null)
                    {
                        if (!callbackOnEndPlayback())
                        {
                            return false;
                        }
                    }
                }
            }

            tries++;

            if (tries > max_tries)
            {
                IsPlaying = false;
                throw new System.ArgumentException("No frames enabled on " + name);
            }
        } while (!enabledAnimationStep[showingIndex]);

        return true;
    }
}
