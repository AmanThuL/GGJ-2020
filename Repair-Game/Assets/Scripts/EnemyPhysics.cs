using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 direction;
    public Vector3 velocity;

    public Vector3 forward;

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
    public float FOV;
    public float attackRadius; // radius of attacking range

    // Stats
    public float hp;
    public float attack;

    public GameObject target;
    public State state;


    // Start is called before the first frame update
    protected void Start()
    {
        position = transform.position;

        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();

        wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
        wanderTicker = 0;

        gameObject.GetComponent<SphereCollider>().radius = FOV;
        state = State.Wander;
    }

    // Update is called once per frame
    protected void Update()
    {

        position = transform.position;

        // New stuff for this (and the next) unit
        transform.forward = rb.velocity.normalized;

        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);

    }

    protected void FixedUpdate()
    {
        switch(state)
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
    //Keep finding random positions to go to, after reaching the position, chill for a moment, then go on
    // ***Does not have keep-in-bound capability***
    public void Wandering()
    {
        //If not at destination, go to destination
        if (!reached)
        {
            RigidGoTo(wanderDestination);
        }

        //If reached
        if (Vector3.Distance(wanderDestination, rb.position) < 0.8f && !reached)
        {
            //if just reached, start cooldown
            if (reached == false)
            {
                wanderTicker += (wanderCooldown + Random.Range(-wanderCooldownOffset, wanderCooldownOffset));
            }

            reached = true;
            rb.velocity *= 0.1f; //Slow down
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

    //Author: Yuan Luo
    //Set velocity towards pos with max speed
    public void GoTo(Vector3 pos)
    {
        velocity = Vector3.ClampMagnitude((pos - position), maxSpeed);
    }

    public void RigidGoTo(Vector3 pos)
    {
        rb.AddForce((pos - rb.position) * 0.05f, ForceMode.VelocityChange);
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

    #region Author: Rachel
    public void Seek(GameObject target)
    {
        RigidGoTo(target.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(target == null)
            {
                target = other.gameObject;
                Debug.Log(gameObject.name + " spotted " + target.name);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(state == State.Wander && other.gameObject.tag == "Player")
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
