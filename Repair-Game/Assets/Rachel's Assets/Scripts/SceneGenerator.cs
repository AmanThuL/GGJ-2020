using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    private int randomIndex;
    private ArrayList sceneArray;

    // Start is called before the first frame update
    void Start()
    {
        sceneArray = new ArrayList();

        // Add scenes here
        sceneArray.Add("Alfie Test Scene");
        sceneArray.Add("Allie Test Scene");
        sceneArray.Add("Main");
        sceneArray.Add("Rudy Test Scene");
        sceneArray.Add("Rachel Test Scene");
        sceneArray.Add("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNewScene();
        }
    }

    void LoadNewScene()
    {
        if(sceneArray.Count > 0)
        {
            randomIndex = Random.Range(0, sceneArray.Count);
            SceneManager.LoadScene((string)sceneArray[randomIndex], LoadSceneMode.Single);
            sceneArray.RemoveAt(randomIndex); // Removing scene from the array ensures it gets loaded only once
            // LoadSceneMode.Single = close current scene and load new scene, so that no scenes overlap
        }
        else
        {
            Debug.Log("No more scenes to load!");
        }
    }
}
