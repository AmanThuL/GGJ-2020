using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
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
    public float wanderCooldown;
    private float wanderTicker;

    // Floats
    public float mass;
    public float maxSpeed;
    public float radius;

    // Stats
    public float hp;
    public float attack;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

        wanderDestination = GetRandomClosePosition(wanderRadius);
        wanderTicker = 0;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Wandering()
    {
        if (!reached)
        {
            GoTo(wanderDestination);
        }
        
        if (Vector3.Distance(wanderDestination, position) < 0.3f)
        {
            reached = true;
            velocity *= 0.8f;
        }
    }

    public void GoTo(Vector3 pos)
    {
        velocity = Vector3.ClampMagnitude((pos - position), maxSpeed);
    }
}
