using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // This method will be called when the player interacts with the object
    public abstract void Interact(Player player);
}
