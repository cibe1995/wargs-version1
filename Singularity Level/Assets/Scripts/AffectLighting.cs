using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectLighting : MonoBehaviour {

    private Animator animator;
    public GameObject door1;
    public GameObject door2;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("it worked...");

        GameObject lightGameObject = new GameObject("The Light");
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.color = Color.blue;
        lightGameObject.transform.position = new Vector3(0, 5, 0);

        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Switch");

        door1.SetActive(false);
        door2.SetActive(false);



   
    }

}
