using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void LifeOrDeath(DeathLife dl, bool byPlayer);

public class DeathLife : MonoBehaviour {

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
            animations[i].Trigger(isAlive ? "Life" : "Death");
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
