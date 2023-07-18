using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    //this script is what the player can do. 
    [SerializeField]
    InputController controller;
    Rigidbody rb;
    CharacterController controller2;

    ObjectShadow shadow;


    MovementKey movement = MovementKey.none;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller2 = GetComponent<CharacterController>();
        shadow = GetComponent<ObjectShadow>();
        activeactions.Add(ActionKey.none);
    }


    //get this thing out of here
    public float speed = 6f;
    public float turnsmoothtime = 0.1f;
    float turnsmoothvelocity;
    public Transform cam;

    Vector3 finalmovement = new Vector3(0, 0, 0);
    void TestCameraMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direksiyon = new Vector3(horizontal, 0f, vertical).normalized;

        if (direksiyon.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(direksiyon.x, direksiyon.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnsmoothvelocity, turnsmoothtime);
            //transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 movedir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            finalmovement = movedir.normalized;
            //controller2.Move(movedir.normalized * speed * Time.deltaTime);
        }
    }

    //test end


    private void OnEnable()
    {
        InputController.PlayerMovementInputEvent += Moveball;
        InputController.PlayerActionInputEvent += PlayerAction;
    }

    private void OnDisable()
    {
        InputController.PlayerMovementInputEvent -= Moveball;
        InputController.PlayerActionInputEvent -= PlayerAction;
    }

    // Update is called once per frame

    private void Update()
    {
        TestCameraMovement();
    }

    private void FixedUpdate()
    {
        SpinBall();
        PlayerAbility();
    }

    //direction and speed of the movement. +
    //get direction from camera
    Vector3 direction = new Vector3();
    [SerializeField]
    float spinforce = 100f;
    [SerializeField]
    float rawforce = 10f;

    //get input from player
    void Moveball(MovementKey key)
    {
        movement = key;
        //if any movement input is given
        if (movement != MovementKey.none && movement != MovementKey.neutral)
        {
            //switch sets the direction of the spin
            switch (movement)
            {
                case MovementKey.neutral:
                    //get forward here
                    return;
                case MovementKey.topleft:
                    direction = new Vector3(1, 0, 1);
                    return;
                case MovementKey.top:
                    direction = new Vector3(1, 0, 0);
                    return;
                case MovementKey.topright:
                    direction = new Vector3(1, 0, -1);
                    return;
                case MovementKey.left:
                    direction = new Vector3(0, 0, 1);
                    return;
                case MovementKey.right:
                    direction = new Vector3(0, 0, -1);
                    return;
                case MovementKey.downleft:
                    direction = new Vector3(-1, 0, 1);
                    return;
                case MovementKey.down:
                    direction = new Vector3(-1, 0, 0);
                    return;
                case MovementKey.downright:
                    direction = new Vector3(-1, 0, -1);
                    return;

            }

        }

    }

    List<ActionKey> activeactions = new List<ActionKey>();
    void PlayerAction(ActionKey key)
    {

        if (activeactions.Contains(key)) { activeactions.Remove(key); }
        else { activeactions.Add(key); }
        Debug.Log("activeactions:" + activeactions[0]);

    }


    void PlayerAbility()
    {
        foreach (ActionKey key in activeactions)
        {
            switch (key)
            {
                case ActionKey.jump:

                    if (shadow.isGrounded)
                    {
                        rb.AddForce(new Vector3(0, 10, 0), ForceMode.VelocityChange);
                        Debug.Log("jump");
                    }

                    break;
                case ActionKey.dash:
                    Dash();
                    break;
                case ActionKey.useItem:
                    break;
                default:
                    break;
            }
        }
    }
    void SpinBall()
    {
        //turns out applying torque makes things go sideways

        Vector3 frontVector = Quaternion.Euler(0f, 90f, 0f) * finalmovement;


        if (movement != MovementKey.none && movement != MovementKey.neutral)
        {
            //Debug.Log(finalmovement);
            rb.AddTorque(frontVector * spinforce, ForceMode.Acceleration);
            rb.angularVelocity *= 0.95f;
            //
            //

            //also add some raw force
            rb.AddForce(finalmovement * rawforce, ForceMode.Acceleration);
        }



    }

    void Dash()
    {
        rb.AddForce(finalmovement * rawforce, ForceMode.VelocityChange);
    }

    public delegate void PlayerPickupsEventHandler(Pickup pickup);
    public event PlayerPickupsEventHandler PlayerPickupEvent;


    public void Interact(Interactables interact)
    {


        if (interact is Pickup pickup)
        {
            if (pickup.Type == pickuptype.gem)
            {

                pickup.Disable();
                //well, what now.
                //this should go right into the counter in gamemanager
                //since gems have nothing to do with the player,this does nothing
                PlayerPickupEvent?.Invoke(pickup);

            }//send this to gamemanager

            if (pickup.Type == pickuptype.powerup)
            {

            }//if powerup is empty,add this. else, 0

            //PlayerPickupEvent?.Invoke(pickup);
        }
    }
}
