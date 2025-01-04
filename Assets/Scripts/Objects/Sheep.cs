using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : InteractableObject
{
    public event EventHandler OnInteracted;

    public override void Interact(Player player)
    {
        OnInteracted?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
