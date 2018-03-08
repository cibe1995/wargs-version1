﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class VRTouchThrusters : MonoBehaviour
{
    //Cannot be used with Gravity!
    [Header("Controls")]
    public OVRInput.Button brakeButton = OVRInput.Button.PrimaryIndexTrigger;
    [Header("-Settings-")]
    [Range(.1f, 3f)]
    public float speedAdd = .5f;
    [Range(.5f, 5)]
    public float maxSpeedMulti = 2;
    [Range(0, 5)]
    public float drag = 0;
    [Range(0,3)]
    public float brakePower = .7f;
    bool isOn;
    bool breaksHit;
    public bool inverted;
    public bool reduceVelocityOnContact = true;
    VRTouchMove2 refSystem;
    Transform storedTransform;
    Vector3 moveDirection;


    public AudioClip thruster; 


    //   // Use this for initialization
    void Start()
    {
        refSystem = GetComponent<VRTouchMove2>();
        if (!refSystem)
        {
            Debug.Log("VRAltMove is not on VRMove object disabling");
            this.enabled = false;
            return;
        }
        refSystem.mainMovementOverRide = true;

        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = thruster;
    }

    //// Update is called once per frame
    void Update()
    {
        if (refSystem.canMove)
        {
            ThrusterMove();
          
        }
    }
    void LateUpdate()
    {
        refSystem.yourRig.Move(moveDirection * Time.deltaTime);
        if(refSystem.yourRig.collisionFlags != CollisionFlags.None)
        {
            if(reduceVelocityOnContact)
            {
                Brakes(.95f);
            }
        }
        if(drag != 0)
        {
            Brakes(drag);
        }
    }

    public void ThrusterMove()
    {
        

        VRTouchMove2.InputData InputHolderDown = refSystem.InputReturnDown(refSystem.ForwardButton);
        VRTouchMove2.InputData InputHolderUp = refSystem.InputReturnUp(refSystem.ForwardButton);
        

        if (InputHolderDown.pressed)
        {
            isOn = true;
            storedTransform = InputHolderDown.selectedController;

            GetComponent<AudioSource>().Play();

        }
        if (isOn)
        {
            

            if (inverted)
            {
                moveDirection += (-storedTransform.forward * speedAdd) * Time.deltaTime;
            }
            else
            {
                moveDirection += (storedTransform.forward * speedAdd) * Time.deltaTime;
            }
            moveDirection = Vector3.ClampMagnitude(moveDirection, refSystem.moveSpeed * maxSpeedMulti);
        }

        if (InputHolderUp.pressed)
        {
            if (InputHolderUp.selectedController == storedTransform)
            {
                isOn = false;
            }
        }
        //Break Area!
        InputHolderDown = refSystem.InputReturnDown(brakeButton);
        InputHolderUp = refSystem.InputReturnUp(brakeButton);
        if (InputHolderDown.pressed)
        {
            breaksHit = true;
        }
        if(breaksHit)
        {
            Brakes(brakePower);
            GetComponent<AudioSource>().Stop();
        }
        if (InputHolderUp.pressed)
        {
                breaksHit = false;
        }
    }

    void Brakes(float chg)
    {
        moveDirection -= (moveDirection * chg) * Time.deltaTime;
    }

}
