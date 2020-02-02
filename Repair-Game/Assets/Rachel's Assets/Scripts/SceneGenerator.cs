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

    public Vector3 spawnPos;

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

        spawnPos = new Vector3(25, 0.5f, 0);

        sceneArray = new ArrayList();
        if (player == null)
        {
            player = Instantiate(playerRef, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(player);
        }

        // Add scenes here
        sceneArray.Add("Sofa");
        sceneArray.Add("Starbuk");
        sceneArray.Add("waterman");
        sceneArray.Add("BasicRoomMs");
        sceneArray.Add("BasicRoomP");
        sceneArray.Add("Room_1Plants");
        sceneArray.Add("Room_2Plants");
        sceneArray.Add("Room_3Plants");
        sceneArray.Add("Room_4Plant");
        sceneArray.Add("Room_5Mar");
        sceneArray.Add("Room_6Mar");
        sceneArray.Add("Room_7Mar");
        sceneArray.Add("Room_8Mar");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNewScene()
    {
        // Remove current scene from the list
        sceneArray.Remove(SceneManager.GetActiveScene().name);

        if (sceneArray.Count > 0)
        {
            randomIndex = Random.Range(0, sceneArray.Count);
            string newSceneName = (string)sceneArray[randomIndex];
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Single); // LoadSceneMode.Single = close previous scene, load new scene | LoadSceneMode.Additive = load scene over old scene
            DelayedExecution(1f, newSceneName);
            //SceneManager.MoveGameObjectToScene(playerRef, SceneManager.GetSceneByName(newSceneName));
            
            sceneArray.RemoveAt(randomIndex); // Removing scene from the array ensures it gets loaded only once
        }
        else
        {
            Debug.Log("No more scenes to load!");
        }

        if(!player.active)
        {
            player.SetActive(true);
        }
    }

    IEnumerator DelayedExecution(float time, string newSceneName)
    {
        yield return new WaitForSeconds(time);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName)); // Sets new active scene
    }
}
