using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInput
{
    //currently pressed movement keys

    //previous movement keys

    //previous action key

    //currently pressed action key

    //previous keys/action reset after some time

    public List<MovementKey> currentmovementkeys = new List<MovementKey>();
    public MovementKey activeVerticalMovement;
    public MovementKey activeHorizontalMovement;


    public MovementKey lastmovement = MovementKey.none;


    public MovementKey lastVerticalMovement = MovementKey.neutral;
    public MovementKey lastHorizontalMovement = MovementKey.neutral;





}


public class InputController : MonoBehaviour
{
    PlayerCharacterInput pinput = new();
    //public delegate void InputEventHandler(MovementKey keypress);


    public delegate void SystemInputEventHandler(SystemKey keypress);
    public delegate void PlayerMovementInputEventHandler(MovementKey keypress);
    public delegate void PlayerActionInputEventHandler(ActionKey keypress);

    public static event SystemInputEventHandler SystemInputEvent;
    public static event PlayerMovementInputEventHandler PlayerMovementInputEvent;
    public static event PlayerActionInputEventHandler PlayerActionInputEvent;

    MovementKey DirectionHandler(MovementKey input1, MovementKey input2)
    {
        switch (input1, input2)
        {

            case (MovementKey.none, MovementKey.none):
                Debug.LogError("no input has given but this is called");
                return MovementKey.none;

            //case for neutral input
            case (MovementKey.neutral, MovementKey.neutral):
                return MovementKey.neutral;

            //cases for one direction inputs
            case (MovementKey.left, MovementKey.neutral):
                return MovementKey.left;

            case (MovementKey.neutral, MovementKey.top):
                return MovementKey.top;

            case (MovementKey.right, MovementKey.neutral):
                return MovementKey.right;

            case (MovementKey.neutral, MovementKey.down):
                return MovementKey.down;


            //cases for diagonal inputs
            case (MovementKey.left, MovementKey.top):
                return MovementKey.topleft;

            case (MovementKey.left, MovementKey.down):
                return MovementKey.downleft;

            case (MovementKey.right, MovementKey.top):
                return MovementKey.topright;

            case (MovementKey.right, MovementKey.down):
                return MovementKey.downright;



        }
        return MovementKey.none;
    }


    void SystemInput() { }

    void ActionInput(ActionKey key)
    {
        PlayerActionInputEvent?.Invoke(key);
    }


    void MovementInput(MovementKey key)
    {
        //get movement key save it 
        pinput.currentmovementkeys.Add(key);
        pinput.lastmovement = key;

        if (key == MovementKey.neutral)
        {
            Debug.Log("how did you do this");
        }

        if (key == MovementKey.left || key == MovementKey.right)
        {
            if (pinput.lastHorizontalMovement == key) { }
            else { pinput.lastHorizontalMovement = key; }

        }
        if (key == MovementKey.top || key == MovementKey.down)
        {
            pinput.lastVerticalMovement = key;
        }



        MovementKey final = DirectionHandler(pinput.lastHorizontalMovement, pinput.lastVerticalMovement);
        PlayerMovementInputEvent?.Invoke(final);
    }

    void MovementInputRelease(MovementKey key)
    {
        pinput.currentmovementkeys.Remove(key);

        if (key == MovementKey.left)
        {
            pinput.lastHorizontalMovement = pinput.currentmovementkeys.Contains(MovementKey.right) ? MovementKey.right : MovementKey.neutral;
        }
        else if (key == MovementKey.right)
        {
            pinput.lastHorizontalMovement = pinput.currentmovementkeys.Contains(MovementKey.left) ? MovementKey.left : MovementKey.neutral;
        }

        if (key == MovementKey.top)
        {
            pinput.lastVerticalMovement = pinput.currentmovementkeys.Contains(MovementKey.down) ? MovementKey.down : MovementKey.neutral;
        }
        else if (key == MovementKey.down)
        {
            pinput.lastVerticalMovement = pinput.currentmovementkeys.Contains(MovementKey.top) ? MovementKey.top : MovementKey.neutral;
        }

        MovementKey final = DirectionHandler(pinput.lastHorizontalMovement, pinput.lastVerticalMovement);



        PlayerMovementInputEvent?.Invoke(final);
    }





    // hard coded player inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            //PlayerInputEvent?.Invoke(SystemKey.pause);
        }

        //movementkey inputs
        if (Input.GetKeyDown("w"))
        {
            MovementInput(MovementKey.top);
        }
        if (Input.GetKeyDown("a"))
        {
            MovementInput(MovementKey.left);
        }
        if (Input.GetKeyDown("s"))
        {
            MovementInput(MovementKey.down);
        }
        if (Input.GetKeyDown("d"))
        {
            MovementInput(MovementKey.right);
        }

        //on key released
        if (Input.GetKeyUp("w"))
        {
            MovementInputRelease(MovementKey.top);
        }
        if (Input.GetKeyUp("a"))
        {
            MovementInputRelease(MovementKey.left);
        }
        if (Input.GetKeyUp("s"))
        {
            MovementInputRelease(MovementKey.down);
        }
        if (Input.GetKeyUp("d"))
        {
            MovementInputRelease(MovementKey.right);
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {

            ActionInput(ActionKey.jump);

        }

        if (Input.GetKeyDown("e"))
        {
            ActionInput(ActionKey.dash);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ActionInput(ActionKey.jump);
        }

        if (Input.GetKeyUp("e"))
        {
            //ActionInput(ActionKey.none);
        }
    }


}




