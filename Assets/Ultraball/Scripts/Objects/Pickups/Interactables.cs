using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public bool interactable = true;

    public virtual void Interact()
    {
        if (!interactable)
        {
            return;
        }



        // Custom interaction logic
    }

    public virtual void Disable()
    {
        interactable = false;
        Destroy(gameObject);
    }

}

public enum pickuptype
{
    gem, powerup, none
}