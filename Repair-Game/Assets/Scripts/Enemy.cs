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

    // Debugging
    //public Material mat1;
    //public Material mat2;
    //public Material mat3;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void Wander()
    {
        // @Alfie implement wander code here
    }

    public Vector3 Separation()
    {
        // TODO
    }

    public Vector3 KeepInBounds()
    {
        // TODO
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
