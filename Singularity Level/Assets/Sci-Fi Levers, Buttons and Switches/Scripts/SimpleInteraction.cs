using UnityEngine;
using System.Collections;

public class SimpleInteraction : MonoBehaviour {

	private Transform cameraT;
	private Animator animator;
	private RaycastHit hit;

	void Start ()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cameraT = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
		
			if(Physics.Raycast(cameraT.position, cameraT.forward, out hit, 8))
			{

				animator = hit.transform.gameObject.GetComponent<Animator>();
				if(animator != null)
				{
				
					animator.SetTrigger("Switch");
				}
			}
		}
	}
}
