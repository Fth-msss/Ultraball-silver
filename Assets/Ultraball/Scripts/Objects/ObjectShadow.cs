using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script handles ground checks and drop shadows.
/// man this needs a redo
/// </summary>
public class ObjectShadow : MonoBehaviour
{
    //layermask is just standable
    [SerializeField] LayerMask standablelayermask;

    //shadow of object
    public GameObject shadow;


    //variables
    [SerializeField]
    SphereCollider objectCollider;

    [SerializeField]
    [Tooltip("length of boxcast")]
    float maxHeightCheck = 500f;

    [SerializeField]
    [Tooltip("for debug")]
    public bool isGrounded = false;

    [SerializeField]
    [Tooltip("how close should object be to a surface to count as grounded")]
    float groundcheckrange = 0.2f;

    public event EventHandler<GroundedEventArgs> grounded;

    [Header("shadow settings")]
    [SerializeField]
    float shadowMaxScale = 1f;
    [SerializeField]
    float shadowMinScale = 0.5f;
    [SerializeField]
    float maximumDistance = 10f;
    [SerializeField]
    float minimumDistance = 1f;

    private void Start()
    {
        if (objectCollider == null) { objectCollider = GetComponent<SphereCollider>(); }


    }

    void Update()
    {






        bool isHit = Physics.BoxCast(objectCollider.bounds.center, objectCollider.bounds.extents - new Vector3(0.06f, 0.06f, 0.1f), new Vector3(0, -1, 0), out RaycastHit hit, Quaternion.identity, maxHeightCheck, standablelayermask);



        if (isHit)
        {
            shadow.SetActive(true);

            MoveShadow(hit.point, hit.distance);


            if (hit.distance <= groundcheckrange) //grounded is true
            {
                if (!isGrounded) //if it was not grounded before
                {
                    isGrounded = true;
                    grounded?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
                }

            }
            else    //grounded is false
            {
                if (isGrounded) //if it was grounded before
                {
                    isGrounded = false;
                    grounded?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
                }
                isGrounded = false;
            }

        }
        else
        {
            if (isGrounded) //if grounded before
            {
                isGrounded = false;
                grounded?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
            }
            //disable shadow when on void
            shadow.SetActive(false);
        }






    }





    //moves and scales objects shadow
    private void MoveShadow(Vector3 position, float distance)
    {


        float shadowscale = 1;

        //determine shadow scale depending on distance 
        if (distance <= minimumDistance)
        {
            shadowscale = 1;
        }
        else if (distance >= maximumDistance)
        {
            shadowscale = 0.5f;
        }
        else
        {
            float percentage = (distance - minimumDistance) / (maximumDistance - minimumDistance) * 100f;
            shadowscale = shadowMaxScale - shadowMinScale / 100 * percentage;
        }

        //apply position and scale changes to shadow
        shadow.transform.localScale = new Vector3(shadowscale, shadowscale, shadowscale - 0.1f);
        shadow.transform.position = new Vector3(transform.position.x, position.y + 0.05f, transform.position.z);





    }



    //gizmos
    /*
    private void OnDrawGizmos()
    {
        Vector3 shadowpoint;
        RaycastHit hit;

        SphereCollider collider = GetComponent<SphereCollider>();
        bool isHit =Physics.BoxCast(collider.bounds.center, collider.bounds.extents - new Vector3(0.06f, 0.06f, 0.1f), new Vector3(0,-1,0), out RaycastHit raycastHit, Quaternion.identity, 1, standablelayermask);


        if (isHit) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, -1, 0) * raycastHit.distance, transform.lossyScale);
        }
        else 
        {
            Gizmos.color = Color.yellow;
         
        }


        RaycastHit hit2;
        float maxheigthcerck=500f;

        bool isHit2 = Physics.Raycast(collider.bounds.center, new Vector3(0.00f, -1f, 0f),out RaycastHit hithit,maxheigthcerck);
      
        if (isHit2) 
        {
            Gizmos.DrawRay(transform.position, new Vector3(0.00f, -1f, 0f) * hithit.distance);
            //Debug.Log(hithit.point);
            shadowpoint = hithit.point;
        }
        else 
        {
            
        }

        float shadowmaxsize = 1;
        float shadowminsize = 0.5f;
        //golgenin maksimum ve minimum degerlerini alacagi distance degerlerini sec
        //maksimum = distance<1
        //minimum = distance>10
        float maximumdistance = 10;

      
        float currentshadowsize = 1;

        //get shadow size. shadow gets smaller when further from ground
        if (hithit.distance <= 1) 
        {
            currentshadowsize = 1;
            //Debug.Log("test:" + currentshadowsize);
        }
        else if (hithit.distance >= 10) 
        {
            currentshadowsize = 0.5f;
            //Debug.Log("test:" + currentshadowsize);
        }
        else 
        {

            float percentage = (hithit.distance-1) / (10-1) * 100f;
            currentshadowsize =  1f-0.5f / 100* percentage;
           // Debug.Log("test:" + currentshadowsize);



        }
        

        //set shadow position and size(on an actual method) 


    }
    */
}

public class GroundedEventArgs : EventArgs
{
    public bool IsGrounded { get; set; }
}