using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// this script handles ground checks and drop shadows.
/// </summary>
public class ObjectShadow : MonoBehaviour
{
    
    [SerializeField] LayerMask standablelayermask;

    //shadow of object
    public GameObject shadow;

    public GameObject OriginObject;

    Collider objectCollider;



    [SerializeField]
    [Tooltip("length of boxcast")]
    float maxHeightCheck = 500f;

    [SerializeField]
    [Tooltip("public for debug")]
    public bool isGrounded = false;

    [SerializeField]
    [Tooltip("how close should object be to a surface to count as grounded")]
    float groundcheckrange = 0.2f;

    [Header("shadow settings")]
    [SerializeField]
    float shadowMaxScale = 1f;
    [SerializeField]
    float shadowMinScale = 0.5f;
    [SerializeField]
    float maximumDistance = 10f;
    [SerializeField]
    float minimumDistance = 1f;

    public event EventHandler<GroundedEventArgs> groundedEvent;

    private void Start()
    {
       
        if (objectCollider == null) { objectCollider = GetComponent<Collider>(); }

        if (OriginObject == null) 
        {
            OriginObject= GameObject.Find("OriginObject");
        }

        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = OriginObject.transform;
        constraintSource.weight = 1;
        shadow.GetComponent<RotationConstraint>().AddSource(constraintSource);

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
                    groundedEvent?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
                }

            }
            else    //grounded is false
            {
                if (isGrounded) //if it was grounded before
                {
                    isGrounded = false;
                    groundedEvent?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
                }
                isGrounded = false;
            }

        }
        else
        {
            if (isGrounded) //if grounded before
            {
                isGrounded = false;
                groundedEvent?.Invoke(this, new GroundedEventArgs { IsGrounded = isGrounded });
            }
            //disable shadow when on void
            shadow.SetActive(false);
        }






    }





    //moves and scales objects shadow
    private void MoveShadow(Vector3 position, float distance)
    {


        float shadowscale = 1;

        //calculate shadow scale depending on distance 
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


}

public class GroundedEventArgs : EventArgs
{
    public bool IsGrounded { get; set; }
}