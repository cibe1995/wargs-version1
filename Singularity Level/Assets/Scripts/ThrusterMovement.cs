using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterMovement : MonoBehaviour {

    public OVRInput.Controller controller;

    public float speed;    public GameObject CenterEyeAnchor;    Rigidbody rb;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) == 1.0)        {            rb.velocity = CenterEyeAnchor.transform.forward * speed * 1; //move the camera forward
            //Debug.Log("trigger squeezed");        }

    }
}
