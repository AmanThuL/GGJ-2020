using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Rachel Wong

public class EnemyPhysics : MonoBehaviour
{
    public enum State
    {
        Wander,
        Defensive,
        Pursuit,
        Attack,
        Dead,
    }

    public Rigidbody rb;
    public Animator animator;

    private Vector3 position;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;

    public Vector3 forward;
    public Vector3 right;

    //Wandering
    public Vector3 wanderDestination;
@ -28,12 +34,16 @@ public class EnemyPhysics : MonoBehaviour
    // Floats
    public float mass;
    public float maxSpeed;
    public float radius;
    public float FOV;
    public float attackRadius; // radius of attacking range

    // Stats
    public float hp;
    public float attack;

    public GameObject target;
    public State state;


    // Start is called before the first frame update
    protected void Start()
@ -45,6 +55,9 @@ public class EnemyPhysics : MonoBehaviour

        wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
        wanderTicker = 0;

        gameObject.GetComponent<SphereCollider>().radius = FOV;
        state = State.Wander;
    }

    // Update is called once per frame
@ -52,8 +65,6 @@ public class EnemyPhysics : MonoBehaviour
{

    position = transform.position;
        
        Debug.DrawLine(position, wanderDestination, Color.yellow);

        // New stuff for this (and the next) unit
        transform.forward = rb.velocity.normalized;
@ -64,7 +75,25 @@ public class EnemyPhysics : MonoBehaviour

    protected void FixedUpdate()
    {
        Wandering();
        switch (state)
        {
            case State.Wander:
                Wandering();
                break;
            case State.Attack:
                // Not yet implemented
                break;
            case State.Defensive:
                // Slowly back away
                break;
            case State.Pursuit:
                Seek(target);
                break;
            case State.Dead:
                // Insert dead animation
                gameObject.SetActive(false);
                break;
        }
    }

    //Author: Yuan Luo
@ -129,4 +158,48 @@ public class EnemyPhysics : MonoBehaviour

        return pos;
    }

#region Author: Rachel
public void Seek(GameObject target)
{
    RigidGoTo(target.transform.position);
}

void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag == "Player")
    {
        if (target == null)
        {
            target = other.gameObject;
            Debug.Log(gameObject.name + " spotted " + target.name);
        }
    }
}

void OnTriggerStay(Collider other)
{
    if (state == State.Wander && other.gameObject.tag == "Player")
    {
        state = State.Defensive;
        Debug.DrawLine(position, target.transform.position, Color.yellow);
    }
}

void OnTriggerExit(Collider other)
{
    state = State.Pursuit;
}

void OnDrawGizmos()
{
    //// FOV range
    //Gizmos.color = Color.green;
    //Gizmos.DrawWireSphere(transform.position, FOV);

    // Attack range
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRadius);
}
    #endregion
}
