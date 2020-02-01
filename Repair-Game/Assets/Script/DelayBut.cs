using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayBut : MonoBehaviour
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

    private bool stay;
    private float stayTime;
    // Start is called before the first frame update
    void Start()
    {        //Allie Zhao
        playerP = player.transform.position;
        //vel = player.transform.forward;
        vel = new Vector3(1, 0, 1).normalized;
        position = transform.position;
        speed = 5;
        butR = 1.0f;
        targetR = 1.0f;
        targetP = new Vector3(10, 1, 10);

        ///
        stay = false;
        stayTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(stay == false) Move();
        CollDect(targetP, targetR);
    }

    //Author: Allie Zhao
    // 3 stage attack
    //The bullet will deatroy after attack 3 times
    private void DelaBult()
    {
        stay = true;
        position = targetP - vel * 1f;
        transform.position = position;
        stayTime -= Time.deltaTime;
        if (stayTime <= 0.6f)
        {
            position = targetP - vel * 0.5f;
            transform.position = position;
        }
        if (stayTime <= 0.3f)
        {
            position = targetP - vel * 0.1f;
            transform.position = position;
        }
        if (stayTime <= 0)
        {
            DestroyBult();
        }
    }

    private void Move()
    {
        position += vel * speed * Time.deltaTime;
        transform.position = position;
    }

    private void CollDect(Vector3 targetPosition, float targectRaadius)
    {
        float dist = Vector3.Distance(targetPosition, position);
        if (dist < butR + targectRaadius)
        {//destory the bullet
         //DestroyBult();
            DestroyBult();
        }
    }
    //Author: Allie Zhao
    //destroy the bullet
    private void DestroyBult()
    {
        gameObject.SetActive(false);

    }
}
