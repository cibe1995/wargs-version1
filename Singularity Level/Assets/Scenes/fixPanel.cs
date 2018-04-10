using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixPanel : MonoBehaviour {

    
    public AudioClip repairComplete;
    public GameObject particleEffect;

	public Light successLight;

	public GameObject pointLights;

	// Use this for initialization
	void Start () {

//        GetComponent<AudioSource>().playOnAwake = false;
//        GetComponent<AudioSource>().clip = fixWeld;
//
//        GetComponent<AudioSource>().playOnAwake = false;
//        GetComponent<AudioSource>().clip = repairComplete;


    }
	
	// Update is called once per frame
	void Update () {
   

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hands"))
        {
            GetComponent<AudioSource>().Play();

            particleEffect.SetActive(false);

			Destroy (pointLights);

			Instantiate (successLight, new Vector3 (-28, 13, 120), Quaternion.identity); 
        }

       

    }
}
