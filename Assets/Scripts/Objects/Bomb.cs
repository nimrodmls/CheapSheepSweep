using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : CollectibleObject
{
    [SerializeField] private BombVisual bombVisual;

    private bool isExploding = false;

    private void Start()
    {
        bombVisual.OnExplodeFinish += BombVisual_OnExplodeFinish;
    }

    private void BombVisual_OnExplodeFinish(object sender, EventArgs e)
    {
        if (!isExploding)
        {
            Debug.LogError("Bomb is not exploding, but visual explode has finished");
            return;
        }

        Destroy(gameObject);
    }

    public override void Collect(Player player)
    {
        Destroy(GetComponent<BoxCollider2D>());
        bombVisual.Explode();
        isExploding = true;
    }
}
