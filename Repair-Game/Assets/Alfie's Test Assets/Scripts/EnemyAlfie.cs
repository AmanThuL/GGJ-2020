using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAlfie : MonoBehaviour
{
    
    private Vector3 position;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;

    public Vector3 forward;
    public Vector3 right;

    //Wandering
    public Vector3 wanderDestination;
    public bool reached; 
    public float wanderRadius;
    public float wanderRadiusOffset;
    public float wanderCooldown; //CD time
    public float wanderCooldownOffset; //CD offset
    private float wanderTicker; //CD tracker

    // Floats
    public float mass;
    public float maxSpeed;
    public float radius;

    // Stats
    public float hp;
    public float attack;


    // Start is called before the first frame update
    protected void Start()
    {
        position = transform.position;

        wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
        wanderTicker = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        Wandering();
        Debug.DrawLine(position, wanderDestination, Color.yellow);
        // The stuff remaining from how we moved vehicles before
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        // New stuff for this (and the next) unit
        direction = velocity.normalized;

        //transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg + 90,0);

        // Zero out acceleration so we start fresh each frame
        acceleration = Vector3.zero;

        // "Draw" the object at its position
        transform.position = position;
    }

    //Author: Yuan Luo
    //Keep finding random positions to go to, after reaching the position, chill for a moment, then go on
    // ***Does not have keep-in-bound capability***
    public void Wandering()
    {
        //If not at destination, go to destination
        if (!reached)
        {
            GoTo(wanderDestination);
        }
        
        //If reached
        if (Vector3.Distance(wanderDestination, position) < 0.8f && !reached)
        {
            //if just reached, start cooldown
            if(reached == false)
            {
                wanderTicker += (wanderCooldown + Random.Range(-wanderCooldownOffset,wanderCooldownOffset));
            }
            
            reached = true;
            velocity *= 0.1f; //Slow down
        }

        if (wanderTicker >= 0) wanderTicker -= Time.deltaTime;

        //if done chilling
        if(wanderTicker <= 0 && reached == true)
        {
            wanderTicker = 0;
            wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset,wanderRadiusOffset));
            reached = false;
        }
    }

    //Author: Yuan Luo
    //Set velocity towards pos with max speed
    public void GoTo(Vector3 pos)
    {
        velocity = Vector3.ClampMagnitude((pos - position), maxSpeed);
    }

    //Auther: Yuan Luo
    //Get a random position within a circle of the instance
    //radius: the radius of the circle
    private Vector3 GetRandomClosePosition(float radius)
    {
        Vector3 pos = Vector3.zero;

        pos = transform.position;

        float angle = Random.Range(0, 360f);
        pos.x += Mathf.Cos(angle) * radius;
        pos.z += Mathf.Sin(angle) * radius;

        return pos;
    }
}
