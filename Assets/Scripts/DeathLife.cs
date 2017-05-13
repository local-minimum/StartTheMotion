using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeDeathCondition { Any, Life, Death, None };

public delegate void LifeOrDeath(DeathLife dl, bool byPlayer);

public class DeathLife : MonoBehaviour {

    public static bool Compatible(LifeDeathCondition condition, bool alive)
    {
        switch (condition)
        {
            case LifeDeathCondition.Any:
                return true;
            case LifeDeathCondition.None:
                return false;
            default:
                return alive == (condition == LifeDeathCondition.Life);
        }
    }

    public event LifeOrDeath OnLifeOrDeath;

    [SerializeField]
    bool _aliveOnAwake = true;

    [SerializeField]
    Bifurcation[] affectedBifurcations;

    [SerializeField]
    bool affectedBifAliveReq;

    bool _alive;

    public bool alive
    {
        get
        {
            return _alive;
        }   
    }

    public void SetAlive(bool isAlive, bool isPlayer)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            string state = isAlive ? "Life" : "Death";
            if (animations[i].HasTrigger(state))
            {
                animations[i].Trigger(state);
            } else
            {
                Debug.Log(animations[i] + " has no trigger " + state);
                if (isAlive)
                {
                    animations[i].Resume();
                } else
                {
                    animations[i].Stop();
                }
            }
            animations[i].GetComponent<SpriteRenderer>().material.SetFloat("_SaturationMixing", isAlive ? 0 : 0.95f);
        }
        _alive = isAlive;
        if (OnLifeOrDeath != null)
        {
            OnLifeOrDeath(this, isPlayer);
        }
    }

    [SerializeField]
    StopMotionAnimator[] animations;

    [SerializeField]
    bool affectAnimationsAtStart;
    private void Start()
    {
        if (affectAnimationsAtStart)
        {
            SetAlive(_aliveOnAwake, false);
        } else
        {
            _alive = _aliveOnAwake;

        }

        for (int i=0; i<affectedBifurcations.Length; i++)
        {
            affectedBifurcations[i].externalCondtion = IsAlive;
        }
    }

    bool IsAlive()
    {
        return alive == affectedBifAliveReq;
    }

    [SerializeField]
    bool carryWhenAlive;

    public bool CanCarry()
    {
        return alive == carryWhenAlive;
    }
}
