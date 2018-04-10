using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CableWarg : MonoBehaviour {

	public AudioClip idle;

	// Use this for initialization
	void Start () {

        
		
	}

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hands"))
        {
            Debug.Log("Success");

			GetComponent<AudioSource>().Play();   
            
			GameObject onLight = new GameObject ("success light");
			Light lightComp = onLight.AddComponent<Light>();
			lightComp.color = Color.white;
			onLight.transform.position = new Vector3 (0, 1, -5);

        }
    }
}
