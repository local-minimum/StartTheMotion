﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrlr : MonoBehaviour {

    [SerializeField]
    Hearts hearts;

    BezierPoint point;

    public float speed = 2;

    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        point = GetComponent<BezierPoint>();
    }

    //bool swappedCurveThisFrame;

    private const float noMove = 0.1f;

    void Orient(MoveDirection direction)
    {
        Vector3 scale = originalScale;
        scale.x *= (direction == MoveDirection.Left ? -1 : 1);
        transform.localScale = scale;
    }
    
    void Update () {

        if (outroAnim != null)
        {
            return;
        }
        //swappedCurveThisFrame = false;

        float hor = IsInControl ? Input.GetAxis("Horizontal") : 0f;
        float dir = 1;
        if (Mathf.Abs(hor) > noMove)
        {
            
            if (hor > noMove)
            {
                Orient(MoveDirection.Right);
                dir = (currentDirection != MoveDirection.Right) ? -1 : 1;
                point.Move(Time.deltaTime * dir * speed * hor);
            }
            else
            {
                Orient(MoveDirection.Left);
                dir = (currentDirection != MoveDirection.Right) ? -1 : 1;
                point.Move(Time.deltaTime * dir * speed * hor);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (interactableDeathLifes.Count > 0)
            {

                DeathLife interactableDeathLife = interactableDeathLifes[interactableDeathLifes.Count - 1];
                if (interactableDeathLife.alive && hearts.CanTakeLife)
                {
                    hearts.TakeLife();
                    interactableDeathLife.SetAlive(false, true);
                } else if (!interactableDeathLife.alive && hearts.CanGiveLife)
                {
                    hearts.GiveLive();
                    interactableDeathLife.SetAlive(true, true);
                }
                
            }
        }
        /* Temp code for action swapping curves 
        if (!swappedCurveThisFrame && changePaths != null && Input.GetButtonDown("Fire1"))
        {
            var target = changePaths.GetTarget<BezierCurve>();
            Debug.Log("Swap to: " + target);
            if (target)
            {
                swappedCurveThisFrame = true;
                float t = target.TimeClosestTo(transform.position);
                if (t == 0)
                {
                    t = Mathf.Epsilon;
                } else if (t == 1)
                {
                    t = 1 - Mathf.Epsilon;
                }
                point.SwapAnchor(target, t);
                
            } 
        } */
	}
    
    BezierZone changePaths;
    MoveDirection currentDirection = MoveDirection.Right;

    public void SetMovementConfig(MoveDirection coordinatingDirection)
    {
        if (coordinatingDirection == MoveDirection.None || coordinatingDirection == currentDirection)
        {
            return;
        }
        Debug.Log(currentDirection);
        currentDirection = coordinatingDirection;
        Orient(currentDirection);
    }

    public bool IsInControl
    {
        get
        {
            return _driver == null;
        }
    }

    DriveMotion _driver;

    void OnPointDriven(DriveMotion driveMotion)
    {
        _driver = driveMotion;
    }

    void OnPointNotDriven(DriveMotion driveMotion)
    {
        if (_driver == driveMotion)
        {
            _driver = null;
        }
    }

    void KillPlayer()
    {
        Kill();
    }

    void KillPlayer(PointInvoker invoker)
    {
        Kill();
    }

    List<DeathLife> interactableDeathLifes = new List<DeathLife>();

    void ShowDeathLife(DeathLife deathLife)
    {
        Debug.Log(name + " can manipulate " + deathLife);
        if (!interactableDeathLifes.Contains(deathLife))
        {
            interactableDeathLifes.Add(deathLife);
        }
    }

    void HideDeathLife(DeathLife deathLife)
    {
        if (interactableDeathLifes.Contains(deathLife))
        {
            interactableDeathLifes.Remove(deathLife);
        }
    }

    void PointDrop(DeathLife lifeDeath)
    {
        point.Detatch();
        outroAnim = FallKill();
        StartCoroutine(outroAnim);

    }

    IEnumerator<WaitForSeconds> outroAnim;

    IEnumerator<WaitForSeconds> FallKill()
    {
        Vector3 speed = Vector3.down * .01f;
        for (int i=0; i<30f; i++)
        {
            speed *= 1.5f;
            transform.position += speed;
            yield return new WaitForSeconds(0.016f);
        }
        Debug.Log("Death fall");
        outroAnim = null;
        Kill();
    }

    [SerializeField]
    bool canDie = true;

    void Kill()
    {
        if (canDie && SpawnPoint.spawnPoint)
        {            
            Debug.Log(name + ": Respawns at " + SpawnPoint.zoneEvent.zone.curve.name);
            point.Attach(SpawnPoint.zoneEvent.zone.curve, SpawnPoint.zoneEvent.zone.center);
        }
    }
}
