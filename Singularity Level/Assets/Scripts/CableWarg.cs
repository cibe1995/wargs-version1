using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CableWarg : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        
		
	}

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("battery"))
        {
            Debug.Log("Zappppppp");

            SceneManager.LoadScene(1);
            

        }
    }
}
