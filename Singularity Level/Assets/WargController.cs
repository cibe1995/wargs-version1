using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WargController : MonoBehaviour {

	private Quaternion rotation = Quaternion.Euler(new Vector3(180,-19,-90));

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log (rotation);
		transform.rotation = rotation;

	}

	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("headset"))
        {
            Debug.Log("Zappppppp");

            SceneManager.LoadScene(1);
            

        }
    }
}
