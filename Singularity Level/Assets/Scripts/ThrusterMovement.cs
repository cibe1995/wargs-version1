using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterMovement : MonoBehaviour {

    public OVRInput.Controller controller;

    public static float speed = 15.0f; //standard speed for the player movement 
    public GameObject CenterEyeAnchor; //find the camera
    public static float orientation = 0.0f; //may not be used....
    public static float rotationSpeed = 10.0f;
    public static float positionalSpeed = 3.0f;

    Rigidbody rb;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody>();
		
	}

    // Update is called once per frame
    void Update() {
        OVRInput.Update(); //updates the state of the controllers

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) == 1.0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) == 1.0) //checking specifically for the set controller trigger this needs to be modified
        {

            Debug.Log("in the loop");

            Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); //looking for the X and Y position of the thumbstick
            Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); //looking for the X and Y position of the secondary thumbstick 

            Quaternion angle = OVRInput.GetLocalControllerRotation(controller); //find the angle of the controllers


            Debug.Log(angle);
           // Debug.Log(primaryAxis);
           // Debug.Log(secondaryAxis);

            rb.velocity = transform.up * positionalSpeed;

            if (primaryAxis.x != 0.0f || primaryAxis.y != 0.0f) //look at the position of the left thumb stick
            {
                //rb.velocity = CenterEyeAnchor.transform.forward * speed * primaryAxis.y + CenterEyeAnchor.transform.right * speed * primaryAxis.x; //change the rigid body velocity on the playercontroller to this updated one

                rb.velocity = CenterEyeAnchor.transform.forward * speed * angle.y + CenterEyeAnchor.transform.right * speed * angle.x;

                Debug.Log("the velocity is: " + rb.velocity);
            }

            else if (primaryAxis.x == 0.0f || primaryAxis.y == 0.0f)
            {
                rb.velocity = CenterEyeAnchor.transform.forward * 0.0f + CenterEyeAnchor.transform.right * 0.0f; //if the primary stick has not moved, then the player does not move

            }

            if (secondaryAxis.x != 0.0f)
             {
                orientation = orientation + rotationSpeed * secondaryAxis.x * Time.deltaTime;
                rb.rotation = Quaternion.Euler(0, orientation, 0);
            }


 

            //Debug.Log(angle);

            //rb.velocity = CenterEyeAnchor.transform.forward * speed * 1;


        }


    }
}
