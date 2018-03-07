using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixPanel : MonoBehaviour {

    public AudioClip fixWeld;
    public GameObject particleEffect;

	// Use this for initialization
	void Start () {

        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = fixWeld;

    }
	
	// Update is called once per frame
	void Update () {
   

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("thruster"))
        {
            GetComponent<AudioSource>().Play();

            particleEffect.SetActive(false);

        }
    }
}
