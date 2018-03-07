using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelWarg : MonoBehaviour {


    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("thruster"))
        {
            Debug.Log("Zappppppp");

            SceneManager.LoadScene(0);


        }
    }
}
