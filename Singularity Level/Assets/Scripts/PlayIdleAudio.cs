using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIdleAudio : MonoBehaviour {

    public AudioClip idle;

	// Use this for initialization
	void Start () {

        GetComponent<AudioSource>().playOnAwake = false;

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Hands")) 
            
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
