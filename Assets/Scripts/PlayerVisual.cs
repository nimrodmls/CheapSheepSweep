using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Transform coinDropoutPrefab;
    [SerializeField] private int coinExplosionCount = 10;

    private void Start()
    {
        Player.Instance.OnBombCollided += Player_OnBombCollided;
    }

    private void Player_OnBombCollided(object sender, EventArgs e)
    {
        CreateCoinExplosion();
    }

    private void CreateCoinExplosion()
    {
        for (int coinIndex = 0; coinIndex < coinExplosionCount; coinIndex++)
        {
            Transform newCoin = Instantiate(coinDropoutPrefab, null);
            newCoin.position = transform.position;
        }
    }
}
