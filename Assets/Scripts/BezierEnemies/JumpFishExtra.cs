using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JumpFishExtra : MonoBehaviour
{

    [SerializeField]
    DeathLife fish;

    BezierPoint fishPoint;

    [SerializeField]
    BezierPoint spawnPoint;

    bool lifeConsumedByPlayer;

    [SerializeField]
    Bifurcation bifucation;

    [SerializeField]
    DeathLife forkReason;

    [SerializeField]
    bool forkCondition;

    [SerializeField]
    List<BezierZone> killers = new List<BezierZone>();
    
    private void OnEnable()
    {
        fishPoint = fish.GetComponent<BezierPoint>();
        bifucation.externalCondtion = Fork;
        fish.OnLifeOrDeath += JumpFishExtra_OnLifeOrDeath;
        forkReason.OnLifeOrDeath += ForkReason_OnLifeOrDeath;
    }

    private void OnDisable()
    {
        if (bifucation.externalCondtion == Fork)
        {
            bifucation.externalCondtion = null;
        }
        fish.OnLifeOrDeath -= JumpFishExtra_OnLifeOrDeath;
        forkReason.OnLifeOrDeath -= ForkReason_OnLifeOrDeath;
    }

    private void ForkReason_OnLifeOrDeath(DeathLife dl, bool byPlayer)
    {
        if (dl.alive && !fish.alive && lifeConsumedByPlayer)
        {
            FallToBottom(false);
        }
    }

    BezierCurve playerKillCurve;

    private void JumpFishExtra_OnLifeOrDeath(DeathLife dl, bool isPlayer)
    {
        if (!isPlayer || killers.Select(e=> e.curve).Contains(fishPoint.Curve))
        {
            return;
        }

        if (fish.alive)
        {
            fishPoint.Attach(playerKillCurve, fishPoint.CurveTime);
            lifeConsumedByPlayer = false;
            playerKillCurve = null;
            fish.GetComponent<StopMotionAnimator>().PlayByName("Flapping");

        } else 
        {
            
            playerKillCurve = fishPoint.Curve;
            fishPoint.Detatch();
            lifeConsumedByPlayer = true;
            
        }
    }

    bool Fork()
    {
        return forkReason.alive == forkCondition;
    }

    bool readyToRessurect;

    [SerializeField]
    BezierZone flappingZone;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (bEvent.type == BezierZoneEventType.EnterZone && bEvent.zone == flappingZone)
        {
            fish.GetComponent<StopMotionAnimator>().PlayByName("Flapping");
            return;
        }

        if (lifeConsumedByPlayer || bEvent.type == BezierZoneEventType.ExitZone)
        {
            return;
        }

        if (!forkReason.alive && killers.Contains(bEvent.zone))
        {
            if (coroutine == null)
            {
                Debug.Log("FallByEvent");
                FallToBottom(true);
            }
            
        }
        else if (forkReason.alive)
        {

            if (!killers.Contains(bEvent.zone))
            {
                if (!fish.alive)
                {
                    lifeConsumedByPlayer = true;
                }
                Debug.Log("DropFishSurfacing");
                sinking = true;
                fish.GetComponent<StopMotionAnimator>().Trigger("Swim");
                fish.transform.rotation = Quaternion.Euler(0, 0, -90f);
                FallToBottom(lifeConsumedByPlayer);
            }
        }
    }

    void StartAtSpawn()
    {
        fish.SetAlive(true, false);
        readyToRessurect = false;
        fishPoint.Attach(spawnPoint.Curve, spawnPoint.CurveTime);
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

    void FallToBottom(bool killFish)
    {
        if (coroutine == null)
        {
            coroutine = _FallToBottom(killFish);
            StartCoroutine(coroutine);
        }
    }

    [SerializeField]
    float fallSpeed = 1f;

    [SerializeField]
    float swimFallSpeed = 8f;

    IEnumerator<WaitForSeconds> _FallToBottom(bool killFish)
    {
        Debug.Log("FishFall");
        fishPoint.Detatch();
        sinking = true;
        if (killFish)
        {
            fish.SetAlive(false, false);
        }
        float wobble = 0.05f;
        float minDelta = 0.1f;
        float dist = 100;
        float tPrev = Time.timeSinceLevelLoad;
        float step = 0.016f;
        while (dist > minDelta)
        {

            Vector3 dir = spawnPoint.transform.position - fish.transform.position;
            dist = dir.magnitude;
            float d = Mathf.Min(dist, (fish.alive ? swimFallSpeed : fallSpeed) * step);
            fish.transform.position += dir.normalized * d + new Vector3(Random.Range(-wobble, wobble), Random.Range(-wobble, wobble));
            yield return new WaitForSeconds(step);

        }
        if (!lifeConsumedByPlayer)
        {
            if (readyToRessurect || fish.alive)
            {
                StartAtSpawn();
            }
            else
            {
                readyToRessurect = !lifeConsumedByPlayer;
            }
        }
        coroutine = null;
        sinking = false;
    }

    bool sinking = false;

    private void Update()
    {
        if (lifeConsumedByPlayer)
        {
            return;
        }

        if (!fish.alive && forkReason.alive)
        {

            if (readyToRessurect && !sinking)
            {
                StartAtSpawn();
            }
            else
            {
                readyToRessurect = !lifeConsumedByPlayer;
            }
        }
    }
}