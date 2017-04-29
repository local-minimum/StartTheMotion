using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMotionSequencer : MonoBehaviour {

    SpriteRenderer sRend;

    [SerializeField]
    Sprite[] sequence;

    [SerializeField]
    bool[] enabledAnimationStep;

    int showingIndex;

    bool animating;

	public void Step()
    {
        animating = false;
        SetNextFrame();
        sRend.sprite = sequence[showingIndex];
    }

    [SerializeField] float m_fps = 15;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        if (!sRend)
        {
            sRend = gameObject.AddComponent<SpriteRenderer>();
        }

        if (enabledAnimationStep.Length < sequence.Length)
        {
            bool[] tmpArr = new bool[sequence.Length];
            System.Array.Copy(enabledAnimationStep, tmpArr, enabledAnimationStep.Length);
            for (int i=enabledAnimationStep.Length; i<tmpArr.Length; i++)
            {
                tmpArr[i] = true;
            }
            enabledAnimationStep = tmpArr;
        }
        Play();
    }

    public void Play()
    {
        if (!animating)
        {
            StartCoroutine(Animate(m_fps));
        }
    }

    public void Stop()
    {
        animating = false;  

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
        animating = true;
        while (animating)
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
            showingIndex++;
            showingIndex %= sequence.Length;
            if (showingIndex == start)
            {
                throw new System.ArgumentException("No frames enabled on " + name);
            }
        } while (!enabledAnimationStep[showingIndex]);
    }
}
