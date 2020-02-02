using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{

    public GameObject melon;
    public GameObject mug;
    public GameObject sofa;

    // Start is called before the first frame update
    void Start()
    {
        if(GameStats.IsThereMug == false)
        {
            mug.SetActive(false);
        }

        if(GameStats.IsThereSofa == false)
        {
            sofa.SetActive(false);
        }

        if(GameStats.IsThereWatermelon == false)
        {
            melon.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
