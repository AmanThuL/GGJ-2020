using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject sceneFader;
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneFader = GameObject.Find("SceneFader");

        if (SceneManager.GetActiveScene().name == "GameT")
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

    public void LoadNextScene()
    {
        Debug.Log("Button clicked");
        sceneFader.SetActive(true);
        sceneFader.GetComponent<SceneFader>().FadeTo(nextSceneName);
    }

}
