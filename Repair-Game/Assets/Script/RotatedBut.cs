using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedBut : MonoBehaviour
{
    //Allie Zhao
    public GameObject player;
    private Vector3 playerP;//player's position OR gun's position
    private Vector3 position;
    private Vector3 vel;//same driction with player froward
    public int speed;//max buullet seppd
    public float butR;
    public float targetR;
    public Vector3 targetP;

    private float angle;
    private float radius;
    private float coldDownTime;



    private float dist;
    private float degree;
    // Start is called before the first frame update
    void Start()
    {
        //Allie Zhao
        playerP = player.transform.position;
        //vel = player.transform.forward;
        vel = new Vector3(1, 0, 1).normalized;
        position = transform.position;
        speed = 5;
        butR = 1.0f;
        targetR = 5.0f;
        targetP = new Vector3(10, 1, 10);
        dist = Random.Range(8f, 12f);
        degree = Random.Range(0f, 180f);
        SetRotatedValue(degree, dist, 20f);
    }


    // Update is called once per frame
    void Update()
    {
        GoAround();
        CollDect(targetP, targetR);
    }

    public void SetRotatedValue(float theAngle, float theRadius, float TheColdDownTime)
    {
        angle = theAngle;
        coldDownTime = TheColdDownTime;
        radius = theRadius;
    }
    //Author: Allie Zhao
    //The bullet will rotated around the player/monster
    //angle----begining angle
    //radius----the distance between shotter and bullet
    public void GoAround()//Allie
    {
        float x = Mathf.Cos(Mathf.Deg2Rad * (angle)) * radius + position.x;//center of the circle
        float z = Mathf.Sin(Mathf.Deg2Rad * (angle)) * radius + position.z;
        //Vector3 temp = new Vector3(x, 10, z);
        //float y = terr.SampleHeight(temp);
        Vector3 newPos = new Vector3(x, 1, z);
        gameObject.transform.position = newPos;
        angle += 1f;
        coldDownTime -= Time.deltaTime;
        if (coldDownTime <= 0)
        {
            DestroyBut();
        }
        //go closter
       
      

    }
    //Author: Allie Zhao
    //destroy the bullet
    private void DestroyBut()
    {
        gameObject.SetActive(false);

    }

    private void CollDect(Vector3 targetPosition, float targectRaadius)
    {
        float dist = Vector3.Distance(targetPosition, transform.position);
        if (dist < butR + targectRaadius)
        {//destory the bullet
         //DestroyBut();
            DestroyBut();
        }
    }
}
