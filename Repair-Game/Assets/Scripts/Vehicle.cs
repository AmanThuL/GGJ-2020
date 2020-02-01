using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    // Vectors necessary for basic movement
    public Vector3 vehiclePosition;
    public Vector3 direction;
    public Vector3 velocity;

    public float mass;
    public float speed;
    public float health;
    public float attack;


    protected void Start()
    {
        vehiclePosition = transform.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.forward = direction;
        vehiclePosition += velocity * Time.deltaTime;
        direction = velocity.normalized;
        transform.position = vehiclePosition;
    }
}
