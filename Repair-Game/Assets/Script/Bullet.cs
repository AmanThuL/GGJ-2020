using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

    // Start is called before the first frame update-----Allie
    void Start()
    {
        //Allie Zhao
        playerP = player.transform.position;
        vel = player.transform.forward;
        //vel = new Vector3(1, 0, 1).normalized;
        position = transform.position;
        speed = 5;
        butR = 1.0f;
        targetR = 1.0f;
        targetP = new Vector3(10, 1, 10);

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //MoveSin();
        //GoCircle(0);
        CollDect(targetP,targetR);
    }
    //Author: Allie Zhao
    //bullet's vel drection is toward the player transform.forwrd
    private void Move()
    {
        position += vel*speed*Time.deltaTime;
        transform.position = position;
    }



    //Author: Allie Zhao
    //use target position - bullet position
    // if their distion is < their radius, that means they are collioned
    private void CollDect( Vector3 targetPosition, float targectRaadius)
    {
        float dist = Vector3.Distance(targetPosition, position);
        if ( dist < butR + targectRaadius)
        {//destory the bullet
         //DestroyBut();
            DestroyBut();
        }
    }
    //Author: Allie Zhao
    //destroy the bullet
    private void DestroyBut()
    {
        gameObject.SetActive(false);

    }

}
