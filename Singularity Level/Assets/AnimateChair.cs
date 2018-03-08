using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateChair : MonoBehaviour {



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "Player")
        {
            collider.GetComponent<Animation>().Play("");
        }
    }
}