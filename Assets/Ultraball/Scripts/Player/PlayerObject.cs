using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    PlayerStats stats;
    PlayerActions actions;
    Rigidbody rb;

    public delegate void PlayerPickupsEventHandler(Pickup pickup);
    public event PlayerPickupsEventHandler PlayerPickupEvent;


    public void Interact(Interactables interact)
    {


        if (interact is Pickup pickup)
        {
            if (pickup.Type == pickuptype.gem)
            {

                pickup.Disable();
                PlayerPickupEvent?.Invoke(pickup);

            }//send this to gamemanager

            if (pickup.Type == pickuptype.powerup)
            {

            }//if powerup is empty,add this. else, 0

            //PlayerPickupEvent?.Invoke(pickup);
        }
    }
}
