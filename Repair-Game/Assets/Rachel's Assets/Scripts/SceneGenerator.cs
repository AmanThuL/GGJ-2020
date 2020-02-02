using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    private int randomIndex;
    private ArrayList sceneArray;
    public GameObject playerPrefab;
    public GameObject playerRef;

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
        // For now, you just need to press the space bar to enter a new scene.
        // This needs to be changed so that when you enter a doorway, a new scene is loaded
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LoadNewScene();
        }
    }

    void LoadNewScene()
    {
        // Remove current scene from the list
        sceneArray.Remove(SceneManager.GetActiveScene().name);

        if(sceneArray.Count > 0)
        {
            randomIndex = Random.Range(0, sceneArray.Count);
            string newSceneName = (string)sceneArray[randomIndex];
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Single); // LoadSceneMode.Single = close current scene and load new scene, so that no scenes overlap
            DelayedExecution(1f, newSceneName);
        }
        else
        {
            Debug.Log("No more scenes to load!");
        }
    }

    IEnumerator DelayedExecution(float time, string newSceneName)
    {
        yield return new WaitForSeconds(time);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName)); // Sets new active scene
        playerRef = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); // Instantiate player character at the center of the new scene (will change the position once all the level layouts are completed)
        sceneArray.RemoveAt(randomIndex); // Removing scene from the array ensures it gets loaded only once
    }
}
