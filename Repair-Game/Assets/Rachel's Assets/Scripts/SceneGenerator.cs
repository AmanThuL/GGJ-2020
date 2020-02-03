using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    public static SceneGenerator sceneGenerator;

    private int randomIndex;
    private ArrayList roomArray;
    private ArrayList tRoomArray;
    public GameObject playerPrefab;
    public static GameObject player;

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
        spawnPos = new Vector3(25, 0.5f, 0);

        roomArray = new ArrayList();
        tRoomArray = new ArrayList();
        if (player == null)
        {
            player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(player);
        }

        // Add scenes here
        tRoomArray.Add("sofaR");
        tRoomArray.Add("Starbuk");
        tRoomArray.Add("waterman");
        //roomArray.Add("BasicRoomMs");
        //roomArray.Add("BasicRoomP");
        roomArray.Add("Room_1Plants");
        roomArray.Add("Room_2Plants");
        roomArray.Add("Room_3Plants");
        roomArray.Add("Room_4Plant");
        roomArray.Add("Room_5Mar");
        roomArray.Add("Room_6Mar");
        roomArray.Add("Room_7Mar");
        roomArray.Add("Room_8Mar");
        roomArray.Add("Room_9Rock1");
        roomArray.Add("Room_10Rock");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNewScene()
    {
        // Remove current scene from the list
        roomArray.Remove(SceneManager.GetActiveScene().name);

        // Load treasure rooms after every four levels
        if(GameStats.Level % 4 == 0)
        {
            LoadNewScene(tRoomArray);
        }
        else
        {
            LoadNewScene(roomArray);
        }

        Debug.Log(GameStats.Level);

        if (!player.active)
        {
            player.SetActive(true);
        }
    }

    public void LoadNewScene(ArrayList rooms)
    {
        if (rooms.Count > 0)
        {
            randomIndex = Random.Range(0, rooms.Count);
            string newSceneName = (string)rooms[randomIndex];
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Single); // LoadSceneMode.Single = close previous scene, load new scene | LoadSceneMode.Additive = load scene over old scene
            DelayedExecution(1f, newSceneName);
            //SceneManager.MoveGameObjectToScene(playerPrefab, SceneManager.GetSceneByName(newSceneName));

            rooms.RemoveAt(randomIndex); // Removing scene from the array ensures it gets loaded only once

            GameStats.Level++;
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
