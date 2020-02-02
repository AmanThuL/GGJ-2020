using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    public static SceneGenerator sceneGenerator;

    private int randomIndex;
    private ArrayList sceneArray;
    public GameObject playerRef;
    public GameObject player;

    void Awake()
    {
        if(sceneGenerator == null)
        {
            sceneGenerator = this;
            DontDestroyOnLoad(sceneGenerator);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(playerRef.name);

        sceneArray = new ArrayList();
        if (player == null)
        {
            player = Instantiate(playerRef, new Vector3(25, 0.5f, 0), Quaternion.identity);
            DontDestroyOnLoad(player);
        }

        // Add scenes here
        sceneArray.Add("Sofa");
        sceneArray.Add("Starbuk");
        sceneArray.Add("waterman");
        //sceneArray.Add("BasicRoomMs");
        //sceneArray.Add("BasicRoomP");
        //sceneArray.Add("Room_1Plants");
        //sceneArray.Add("Room_2Plants");
        //sceneArray.Add("Room_3Plants");
        //sceneArray.Add("Room_4Plant");
        //sceneArray.Add("Room_5Mar");
        //sceneArray.Add("Room_6Mar");
        //sceneArray.Add("Room_7Mar");
        //sceneArray.Add("Room_8Mar");
    }

    // Update is called once per frame
    void Update()
    {
        // For now, you just need to press tab to enter a new scene.
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
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Single); // LoadSceneMode.Single = close previous scene, load new scene | LoadSceneMode.Additive = load scene over old scene
            DelayedExecution(1f, newSceneName);
            SceneManager.MoveGameObjectToScene(playerRef, SceneManager.GetSceneByName(newSceneName));
            sceneArray.RemoveAt(randomIndex); // Removing scene from the array ensures it gets loaded only once
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
    }
}
