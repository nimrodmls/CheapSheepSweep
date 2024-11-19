using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombVisual : MonoBehaviour
{
    public event EventHandler OnExplodeFinish;

    private const string BOMB_HIT = "BombHit";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Explode()
    {
        animator.SetTrigger(BOMB_HIT);
    }

    // This is called upon the explosion animation finishing
    public void ExplodeFinish()
    {
        OnExplodeFinish?.Invoke(this, EventArgs.Empty);
    }
}
