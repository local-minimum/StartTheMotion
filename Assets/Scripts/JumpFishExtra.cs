using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFishExtra : MonoBehaviour
{

    [SerializeField]
    DeathLife fish;

    bool lifeConsumedByPlayer;

    [SerializeField]
    Bifurcation bifucation;

    [SerializeField]
    DeathLife forkReason;

    [SerializeField]
    bool forkCondition;

    private void OnEnable()
    {
        bifucation.externalCondtion = Fork;
    }

    private void OnDisable()
    {
        if (bifucation.externalCondtion == Fork)
        {
            bifucation.externalCondtion = null;
        }
    }


    bool Fork()
    {
        Debug.Log("Fork test " + (forkReason.alive == forkCondition));
        return forkReason.alive == forkCondition;
    }

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (!forkReason.alive)
        {
            StopFallToBottom();
            fish.alive = false;
        }     
    }

    void StopFallToBottom()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        
    }

    IEnumerator<WaitForSeconds> coroutine;
}