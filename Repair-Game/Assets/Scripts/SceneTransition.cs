using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public GameObject sceneFader;

    // Start is called before the first frame update
    void Start()
    {
        sceneFader = GameObject.Find("SceneFader");
        StartCoroutine(TransitionToIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TransitionToIntro()
    {
        yield return new WaitForSeconds(4);
        sceneFader.GetComponent<SceneFader>().FadeTo("Intro");
    }
}
