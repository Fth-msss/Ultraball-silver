using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    PlayerStats stats;
    PlayerController actions;
    Rigidbody rb;

    public delegate void PlayerPickupsEventHandler(Pickup pickup);
    public event PlayerPickupsEventHandler PlayerPickupEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        actions = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }


    public void Interact(Interactables interact)
    {


        if (interact is Pickup pickup)
        {
            if (pickup.Type == pickuptype.gem)
            {

                pickup.Disable();
                PlayerPickupEvent?.Invoke(pickup);

            }

            if (pickup.Type == pickuptype.powerup)
            {
                //pick powerups only if 
                if (stats.HeldPickup != null) { pickup.Disable(); stats.HeldPickup = pickup; }
                
            }

          
        }
    }

   
    public void AddExternalForce(Vector3 force) 
    {
    rb.AddForce(force);
    }
}
