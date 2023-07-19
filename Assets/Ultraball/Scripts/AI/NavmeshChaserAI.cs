using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshChaserAI : NavmeshEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        SetTargetLocation("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player")) 
        {
        //send them to brazil
        }
    }
}
