using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class WargController : MonoBehaviour {

	private Quaternion rotation = Quaternion.Euler(new Vector3(180,151,-90));

	public VideoPlayer cutscene; 
	public AudioClip wargingaudio;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log (rotation);
		transform.rotation = rotation;

	}

	IEnumerator OnTriggerEnter(Collider other)
    {
	
        if(other.gameObject.CompareTag("headset"))
        {
            Debug.Log("Zappppppp");

			GetComponent<AudioSource>().Play();

			cutscene.Play();

		yield return new WaitForSeconds(8);

           SceneManager.LoadScene(1);
            

        }
    }
}
