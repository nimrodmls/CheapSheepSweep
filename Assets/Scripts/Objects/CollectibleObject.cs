using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleObject : MonoBehaviour
{

    // This method will be called when the player collects the object
    public abstract void Collect(Player player);
}
