using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : InteractableObject
{
    [SerializeField] private CoinsCollectedUI coinCollectorUI;

    public override void Interact(Player player)
    {
        coinCollectorUI.CollectCoin(transform.position);
    }
}
