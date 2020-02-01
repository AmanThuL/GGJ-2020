using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    private Vector3 position;
    public float coldDownTime;
    private float coldtimer;
    public GameObject roBullet;

    private float autoTime;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        coldDownTime = 0.5f;
        coldtimer = coldDownTime;

        autoTime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        ShootingColdDown();
    }
    //Author: Allie Zhao
    //there is a cold dow time, which we can change it
    // the clod down TimER, is the counter
    // the player can only shoot after the timer is < 0
    private void ShootingColdDown()
    {
        coldtimer -= Time.deltaTime;
        if (coldtimer<=0&&Input.GetKeyDown(KeyCode.S))
        {
            Shoot();
        
            coldtimer = coldDownTime;
        }
        if (coldtimer <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            Fire();
            coldtimer = coldDownTime;
        }
    }

    //Author: Allie Zhao
    //creact the bullet
    private void Shoot()
    {
        Debug.Log("shoot");
        Instantiate(bullet, position, Quaternion.identity);
    }

    //Author: Allie Zhao
    private void Fire()
    {   
        GameObject but1 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but2 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but3 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but4 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but5 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but6 = Instantiate(roBullet, position, Quaternion.identity);
        GameObject but7 = Instantiate(roBullet, position, Quaternion.identity);
    }

    private void AutoShoot()
    {
        autoTime -= Time.deltaTime;
        if (autoTime <= 0)
        {
            Shoot();
            autoTime = 3f;
        }
    }
}
