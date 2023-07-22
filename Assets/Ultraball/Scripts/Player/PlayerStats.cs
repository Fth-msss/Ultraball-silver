using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //things like speed, held item here
    //also has events 
    

    //static variables
    [SerializeField]
     float jumppower;
    [SerializeField]
     float rotationpower;
    [SerializeField]
     float bounciness;


    Pickup heldPickup;

    //events

    public delegate void PowerupChangeEventHandler(Pickup pickup);
    public static event PowerupChangeEventHandler PowerupchangeEvent;

    public Pickup HeldPickup
    {

        get { return heldPickup; }

        set { heldPickup = value; PowerupchangeEvent?.Invoke(heldPickup); }

    }

    public float Jumppower { get => jumppower; }
    public float Rotationpower { get => rotationpower; }
    public float Bounciness { get => bounciness; }

}
