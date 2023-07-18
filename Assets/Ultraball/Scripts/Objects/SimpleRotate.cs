using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    //rotate object attached
    [SerializeField]
    Vector3 rotatespeed;


    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(rotatespeed.x, rotatespeed.y, rotatespeed.z));
    }
}
