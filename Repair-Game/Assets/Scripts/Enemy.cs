using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Rachel Wong

public class Enemy : Vehicle
{
    public GameObject target;
    public GameObject enemyPrefab;

    public float FOV;
    public float attackRadius;
    public float avoidRadius;

    //Author: Yuan Luo
    //Wandering
    public Vector3 wanderDestination;
    public bool reached;
    public float wanderRadius;
    public float wanderRadiusOffset;
    public float wanderCooldown; //CD time
    public float wanderCooldownOffset; //CD offset
    private float wanderTicker; //CD tracker

    // Debugging
    //public Material mat1;
    //public Material mat2;
    //public Material mat3;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
        wanderTicker = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Wander();
        base.Update();
    }

    //Author: Yuan Luo
    //Keep finding random positions to go to, after reaching the position, chill for a moment, then go on
    // ***Does not have keep-in-bound capability***
    public void Wander()
    {
        //If not at destination, go to destination
        if (!reached)
        {
            GoTo(wanderDestination);
        }

        //If reached
        if (Vector3.Distance(wanderDestination, vehiclePosition) < 0.8f && !reached)
        {
            //if just reached, start cooldown
            if (reached == false)
            {
                wanderTicker += (wanderCooldown + Random.Range(-wanderCooldownOffset, wanderCooldownOffset));
            }

            reached = true;
            velocity *= 0.1f; //Slow down
        }

        if (wanderTicker >= 0) wanderTicker -= Time.deltaTime;

        //if done chilling
        if (wanderTicker <= 0 && reached == true)
        {
            wanderTicker = 0;
            wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
            reached = false;
        }
    }

    public Vector3 Separation()
    {
        // TODO
        return Vector3.zero;
    }

    public Vector3 KeepInBounds()
    {
        // TODO
        return Vector3.zero;
    }

    //Author: Yuan Luo
    //Set velocity towards pos with max speed
    //pos: position to go to
    public void GoTo(Vector3 pos)
    {
        velocity = Vector3.ClampMagnitude((pos - vehiclePosition), speed);
    }

    /// <summary>
    /// Seeking steering force
    /// </summary>
    public Vector3 Seek(Vector3 targetPosition)
    {
        // Step 1: Find DV (desired velocity)
        // TargetPos - CurrentPos
        Vector3 desiredVelocity = targetPosition - vehiclePosition;

        // Step 2: Scale vel to speed
        // desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);
        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * speed;

        // Step 3:  Calculate seeking steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        return seekingForce;
    }

    /// <summary>
    /// Seek helper method
    /// </summary>
    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    /// <summary>
    /// Obstacle avoidance steering behavior
    /// </summary>
    /// <param name="obstacle"></param>
    /// <returns></returns>
    public Vector3 AvoidObstacles(GameObject obstacle)
    {
        Vector3 vectorToObstacle = obstacle.transform.position - transform.position;
        float minDistance = obstacle.GetComponent<Obstacle>().radius + avoidRadius;

        // Is obstacle behind?
        if (Vector3.Dot(transform.forward, vectorToObstacle) < 0)
        {
            return Vector3.zero;
        }
        // Far enough ahead?
        if (vectorToObstacle.sqrMagnitude > minDistance * minDistance)
        {
            return Vector3.zero;
        }
        // Far enough to the right/left?
        if (Mathf.Abs(Vector3.Dot(transform.right, vectorToObstacle)) >= minDistance)
        {
            return Vector3.zero;
        }

        // If all fails...
        // Is obstacle on the right?
        if (Vector3.Dot(transform.right, vectorToObstacle) > 0)
        {
            return -(transform.right * speed);
        }
        else
        {
            return transform.right * speed;
        }
    }

    //<Helper Function>
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

    /// <summary>
    /// I'm leaving this here in case we want to add debug lines - Rachel Wong
    /// </summary>
    //void OnRenderObject()
    //{
    //    if(sceneManager.debugLinesOn)
    //    {
    //        // Set the material to be used for the first line
    //        mat1.SetPass(0);
    //        // Draws one line
    //        GL.Begin(GL.LINES); // Begin to draw lines
    //        GL.Vertex(transform.position); // First endpoint of this line
    //        GL.Vertex(transform.position + (transform.forward * 3f)); // Second endpoint of this line
    //        GL.End(); // Finish drawing the line

    //        // Set another material to draw the right line in a different color
    //        mat2.SetPass(0);
    //        GL.Begin(GL.LINES);
    //        GL.Vertex(transform.position);
    //        GL.Vertex(transform.position + (transform.right * 3f));
    //        GL.End();

    //        mat3.SetPass(0);
    //        GL.Begin(GL.LINES);
    //        GL.Vertex(transform.position);
    //        if (target != null)
    //        {
    //            GL.Vertex((target.transform.position - transform.position)+ transform.position);
    //        }
    //        GL.End();

    //        futureSphere.GetComponent<Renderer>().enabled = true;
    //    }
    //    else
    //    {
    //        futureSphere.GetComponent<Renderer>().enabled = false;
    //    }
    //}
}
