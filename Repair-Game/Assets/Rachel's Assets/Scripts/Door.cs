using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.SetActive(false);
            other.gameObject.transform.position = new Vector3(25, 0.5f, 0);
            sm.GetComponent<SceneGenerator>().LoadNewScene();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
        }
    }
}
