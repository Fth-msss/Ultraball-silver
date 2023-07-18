using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactables
{


    [SerializeField]
    pickuptype type;

    public pickuptype Type { get => type; }

    public override void Interact()
    {
        if (interactable)
        {
            Debug.Log("pickup touched");
            base.Interact();
            //object specific logic here



        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactable)
            {
                other.GetComponent<PlayerActions>().Interact(this);
            }


        }
    }


}
