 /*******************************************************
 * Copyright (C) 2016 3lbGames
 * 
 * VRTouchMove
 *
 * VRTouchMove can not be copied and/or distributed without the express
 * permission of 3lbGames

 For additional Information or unity development services please contact services@3lbgames.com
 
 For technical support contact support@3lbgames.com

 DoTween is being used to help with easing and simulator sickness reduction.
 
 *******************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class VRTouchMove2 : MonoBehaviour
{

    public enum eControllerType { Left,Right,Both };
    [Header("-Movement Control Settings-")]
    public eControllerType ControlsOn;
    OVRInput.Controller myController;  //Controller Choice
    public OVRInput.Button ForwardButton;        //Button for Default Movement
    public OVRInput.Button BackwardButton;      //Backwards Button


    [Header("-Movement Modes-")]
    public bool canMove = true;
    public enum eMovementMode { Flight, Grounded, None,NoneWithGravity, Remote, Controller, Keyboard};
    public eMovementMode MovementMode = eMovementMode.Flight;
    public bool gravityInDebug = false;

    [Header("-General Settings-")]
    public float moveSpeed = 5;               // Your MovementSpeed shared across all Movement Systems
    public float PlayerGravity = 50;          //Player Gravity for the FPS Controller
	[Range(1, 10)]
	public float basicAcceleration = .8f;             //Acceleration Curve 
    [Header("-Fade Settings-")]
    public float fadeTime = .3f;
    //[Header("-Acceleration Settings-")]
    bool accelSpeed = true;            //Enable Speed Acceleration
    float decay = .9f;                 //Speed Decay 


    float acc = .1f;
    [Header("-Hookups-")]
    public CharacterController yourRig;      //Ensure the Charactor Controller is the correct size for your play space
    public bool AutoAssignTheRest = true;
    public Transform headRig;                //Slot for the Center Camera
    public Transform leftController;        //Use either a touch controller or the headRig;
    public Transform rightController;        //Use either a touch controller or the headRig;
    public VRFadeScript myFade;
    float curSpeed;
    bool gravityOverRide = false;
    [HideInInspector]
    public bool mainMovementOverRide;
    Transform selectedController;        //Use either a touch controller or the headRig;

    //Slowed Start to ensure you get all required controllers;
    IEnumerator Start()
    {

        if (AutoAssignTheRest)
        {
            canMove = false;
            yield return new WaitForSeconds(Time.deltaTime);
            OVRCameraRig ObjectHolder = yourRig.GetComponent<OVRCameraRig>();
            leftController = ObjectHolder.leftHandAnchor;
            rightController = ObjectHolder.rightHandAnchor;
            headRig = ObjectHolder.centerEyeAnchor;
            if (headRig.GetComponent<VRFadeScript>())
            {
                myFade = headRig.GetComponent<VRFadeScript>();
            }
            else
            {
                myFade = headRig.gameObject.AddComponent<VRFadeScript>();
            }
        }
        myFade = headRig.gameObject.AddComponent<VRFadeScript>();
        canMove = true;

    }

    //Returns Player's Height for Teleportation
    public float GetHeight()
    {
        float holder;
        if(yourRig.height > yourRig.radius)
        {
            holder = yourRig.height;
        }
        else
        {
            holder = yourRig.radius/2;
        }
        return holder;
    }
  

    // Update is called once per frame
    void Update()
    {
        //Check Movement;
        //Note Debug Flight will OverRide Everything!
        if (canMove)
        {
            if (MovementMode == eMovementMode.Controller || MovementMode == eMovementMode.Remote || MovementMode == eMovementMode.Keyboard)
            {
                DebugMovement();
            }
            //Module will overRide Basic Movement
            if (mainMovementOverRide)
            {
                return;
            }
            if (MovementMode == eMovementMode.NoneWithGravity)
            {
                ApplyGravity();
            }
            if(MovementMode == eMovementMode.Flight || MovementMode == eMovementMode.Grounded)
            {
                MoveInputSystem();
                if(MovementMode == eMovementMode.Grounded)
                {
                    ApplyGravity();
                }
            }
        }
    }


    //Apply Gravity if not Grounded
    public void ApplyGravity()
    {
        if(!gravityOverRide)
        {
            if (MovementMode == eMovementMode.Grounded || MovementMode == eMovementMode.NoneWithGravity || MovementMode == eMovementMode.Keyboard || MovementMode == eMovementMode.Remote || MovementMode == eMovementMode.Controller)
            {
                if (!yourRig.isGrounded)
                {
                    Vector3 holder = Vector3.zero;
                    holder.y -= PlayerGravity * Time.deltaTime * moveSpeed;
                    //Apply Movement
                    yourRig.Move(holder * Time.deltaTime);
                }
            }
        }

    }
    //Gives gravity to modules
    public float GetGravity()
    {
        return -PlayerGravity * Time.deltaTime * moveSpeed;
    }

    //Detect Inputs on Which Hand
    void MoveInputSystem()
    {
        float v = 0;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                if (OVRInput.Get(ForwardButton, OVRInput.Controller.LTouch))
                {
                    v = GetAxisFromButton(ForwardButton, OVRInput.Controller.LTouch);
                }
                //Detect Left Backward
                if (OVRInput.Get(BackwardButton, OVRInput.Controller.LTouch))
                {
                    v = GetAxisFromButton(BackwardButton, OVRInput.Controller.LTouch, false); 
                }
                AdvancedMove(leftController, v);
                break;
            //RIGHT
            case eControllerType.Right:
                if (OVRInput.Get(ForwardButton, OVRInput.Controller.RTouch))
                {
                    v = GetAxisFromButton(ForwardButton, OVRInput.Controller.RTouch);
                }
                //Detect Left Backward
                if (OVRInput.Get(BackwardButton, OVRInput.Controller.RTouch))
                {
                    v = GetAxisFromButton(BackwardButton, OVRInput.Controller.RTouch, false);
                }
                AdvancedMove(rightController, v);
                break;
            ///BOTH;
            case eControllerType.Both:
                if (OVRInput.Get(ForwardButton, OVRInput.Controller.LTouch))
                {
                    v = GetAxisFromButton(ForwardButton, OVRInput.Controller.LTouch);
                }
                //Detect Left Backward
                if (OVRInput.Get(BackwardButton, OVRInput.Controller.LTouch))
                {
                    v = GetAxisFromButton(BackwardButton, OVRInput.Controller.LTouch, false);
                }
                if(v != 0)
                {
                    AdvancedMove(leftController, v);
                }
                v = 0;
                if (OVRInput.Get(ForwardButton, OVRInput.Controller.RTouch))
                {
                    v = GetAxisFromButton(ForwardButton, OVRInput.Controller.RTouch);
                }
                //Detect Left Backward
                if (OVRInput.Get(BackwardButton, OVRInput.Controller.RTouch))
                {
                    v = GetAxisFromButton(BackwardButton, OVRInput.Controller.RTouch, false);
                }
                if (v != 0)
                {
                    AdvancedMove(rightController, v);
                }
                break;
        }
    }
    //Advanced Movement System for flight and grounded systems
    public void AdvancedMove(Transform thisController,float speed)
    {
		acc = moveSpeed * basicAcceleration;
        float v = speed; //Place Holder for Foward Stick
        if (accelSpeed)
        {
            //Decay Speed
            if (Mathf.Abs(v) <= 0)
            {
                curSpeed *= decay;
            }
            else
            {
                //Apply Acceloration
                curSpeed += acc * v * Time.deltaTime;
                //curSpeed += acc * v2 * Time.deltaTime;
            }
            curSpeed = Mathf.Clamp(curSpeed, -moveSpeed, moveSpeed);
        }
        else
        {
            curSpeed = moveSpeed * v;
        }
        curSpeed = Mathf.Clamp(curSpeed, -curSpeed * v, curSpeed * v);
        Vector3 holder = thisController.forward;
        if(MovementMode == eMovementMode.Grounded)
        {
            holder.y = 0;
        }
        yourRig.Move(holder * curSpeed * Time.deltaTime);
    }


    /// <summary>
    ///Debug Flight
    ///      This is the Main Function for the Movement system. It is designed to be an arcade flight controller in order to navigate around in a space. 
    ///      Press the Move button and you are dragged foward where ever the touch controller is pointed.
    /// </summary>
    public void DebugMovement()
    {
        float v = 0;
        float h = 0;
        Vector3 moveDirection;
        switch (MovementMode)
        {
            case eMovementMode.Remote:

                if (OVRInput.Get(OVRInput.Button.Up, OVRInput.Controller.Remote))
                {
                    v = 1;
                }
                if (OVRInput.Get(OVRInput.Button.Down, OVRInput.Controller.Remote))
                {
                    v = -1;
                }
                if (OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.Remote))
                {
                    RotateByDegrees(-45);
                }
                if (OVRInput.GetDown(OVRInput.Button.Right, OVRInput.Controller.Remote))
                {
                    RotateByDegrees(45);
                }

                moveDirection = (headRig.forward * v) * moveSpeed * Time.deltaTime;
                if (gravityInDebug)
                {
                    moveDirection.y = 0;
                }
                yourRig.Move(moveDirection);
                if (gravityInDebug)
                {
                    ApplyGravity();
                }
                break;
            case eMovementMode.Controller:

                v = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Gamepad).y;
                h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Gamepad).x;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryShoulder, OVRInput.Controller.Gamepad))
                {
                    RotateByDegrees(-45);
                }
                if (OVRInput.GetDown(OVRInput.Button.SecondaryShoulder, OVRInput.Controller.Gamepad))
                {
                    RotateByDegrees(45);
                }
                moveDirection = headRig.TransformDirection(new Vector3(h, 0, v)) * moveSpeed * Time.deltaTime;
                if (gravityInDebug)
                {
                    moveDirection.y = 0;
                }
                yourRig.Move(moveDirection);
                if (gravityInDebug)
                {
                    ApplyGravity();
                }
                break;
            case eMovementMode.Keyboard:
                v = Input.GetAxis("Vertical");
                h = Input.GetAxis("Horizontal");
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    RotateByDegrees(-45);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RotateByDegrees(45);
                }
                //Your own Speed
                //float speedAdd = 5;
                //if (Input.GetKey(KeyCode.LeftShift))
                //{
                //    speedAdd = moveSpeed;
                //}
                moveDirection = headRig.TransformDirection(new Vector3(h, 0, v)) * (moveSpeed) * Time.deltaTime;
                if (gravityInDebug)
                {
                    moveDirection.y = 0;
                }
                yourRig.Move(moveDirection);
                if (gravityInDebug)
                {
                    ApplyGravity();
                }
                break;
            default:
                break;

        }
    }
    //Helper function that returns the Axis when set to a button
    float GetAxisFromButton(OVRInput.Button theButton ,OVRInput.Controller theController,bool isPositive = true)
    {
        float holder = 0;
        switch (theButton)
        {
            case OVRInput.Button.PrimaryHandTrigger:
                holder = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, theController);
                break;
            case OVRInput.Button.PrimaryIndexTrigger:
                holder = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, theController);
                break;
            case OVRInput.Button.SecondaryThumbstickUp:
                holder = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, theController).x;
                break;
            case OVRInput.Button.Down:
                holder = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, theController).x;
                break;
            default:
                holder = 1; //This return is to for things without Axises
                break;
        }
        if(!isPositive)
        {
            holder *= -1;
        }
        return holder;
    }

    //Helper Function to do Rotation
    public void RotateByDegrees(float degrees)
    {
        if (myFade)
        {
            myFade.StartFadeIn(fadeTime);
        }
        Vector3 holder1;
        holder1 = yourRig.transform.rotation.eulerAngles;
        holder1.y += degrees;
        Vector3 rotPosition = holder1;
        yourRig.transform.DORotate(rotPosition,0);
    }

    //Input Data Structure for Modules
    public struct InputData
    {
        public bool pressed;
        public Transform selectedController;
        public bool isLeft;
        public bool isRight;
    }

    //Get InputData Function
    public InputData InputReturnGet(OVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        holder.isLeft = false;
        holder.isRight = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (OVRInput.Get(myButton, OVRInput.Controller.LTouch))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (OVRInput.Get(myButton, OVRInput.Controller.RTouch))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:
                if (OVRInput.Get(myButton, OVRInput.Controller.LTouch))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;

                }
                if (OVRInput.Get(myButton, OVRInput.Controller.RTouch))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
    //Down InputData Function
    public InputData InputReturnDown(OVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (OVRInput.GetDown(myButton, OVRInput.Controller.LTouch))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (OVRInput.GetDown(myButton, OVRInput.Controller.RTouch))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:

                if (OVRInput.GetDown(myButton, OVRInput.Controller.LTouch))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;
                }
                if (OVRInput.GetDown(myButton, OVRInput.Controller.RTouch))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
    //Up InputData Function
    public InputData InputReturnUp(OVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (OVRInput.GetUp(myButton, OVRInput.Controller.LTouch))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (OVRInput.GetUp(myButton, OVRInput.Controller.RTouch))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:

                if (OVRInput.GetUp(myButton, OVRInput.Controller.LTouch))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;
                }
                if (OVRInput.GetUp(myButton, OVRInput.Controller.RTouch))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
}
