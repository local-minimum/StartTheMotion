using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

struct HeartAndMaterial{

    public StopMotionSequencer sequencer;
    public Material mat;

    public HeartAndMaterial(StopMotionSequencer sequencer)
    {
        this.sequencer = sequencer;
        mat = sequencer.GetComponent<SpriteRenderer>().material;
    }
    
}

public class Hearts : MonoBehaviour {

    [SerializeField, Range(0,1)]
    float deadSaturationMixing = 1;

    [SerializeField, Range(0, 1)]
    float aliveSaturationMixing = 0.2f;

    HeartAndMaterial[] hearts;

    int aliveHearts = 0;

    void Start () {
        hearts = GetComponentsInChildren<StopMotionSequencer>().Select(e => new HeartAndMaterial(e)).ToArray();
        KillAll();

        //TODO: Fix better
        Debug.Log("Takes life: " + TakeLife());
	}
	
    void KillAll()
    {
        for (int i=0, l = hearts.Length; i<l; i++)
        {
            KillHeart(i);
        }
        aliveHearts = 0;
    }

    void KillHeart(int i)
    {
        hearts[i].sequencer.Stop();
        hearts[i].mat.SetFloat("SaturationMixing", deadSaturationMixing);
    }

    void RessurectHeart(int i)
    {
        hearts[i].sequencer.Play(false);
        hearts[i].mat.SetFloat("SaturationMixing", aliveSaturationMixing);
    }

    public bool CanGiveLife
    {
        get
        {
            return aliveHearts > 0;
        }
    }

    public bool CanTakeLife
    {
        get
        {
            return aliveHearts < hearts.Length;
        }
    }

    public bool GiveLive()
    {
        if (CanGiveLife)
        {
            aliveHearts--;
            KillHeart(aliveHearts);
            return true;
        }
        return false;
    }

    public bool TakeLife()
    {
        if (CanTakeLife)
        {
            RessurectHeart(aliveHearts);
            aliveHearts++;
            return true;
        }
        return false;
    }

    void Update () {
		
	}
}
