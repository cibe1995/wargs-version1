using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class failurePoint : MonoBehaviour {

	public GameObject littleLight;

	// Use this for initialization
	void Start () {
		
		
	}

	// Update is called once per frame
	void Update () {
		CreateandDestroy ();

	}

	public void CreateandDestroy() {
		Debug.Log ("in the function");
		Instantiate (littleLight, new Vector3 (-2, 0, 4), Quaternion.identity);
		//yield return new WaitForSeconds(2);
		Destroy (littleLight);
	}
}
