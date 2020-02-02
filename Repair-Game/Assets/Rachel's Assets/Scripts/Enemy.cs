using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
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

    public Vector3 acceleration;
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
    protected float wanderTicker; //CD tracker

    // Floats
    public float mass;
    public float maxSpeed;
    public float fov;
    public float attackRadius;

    // Stats
    public float hp;
    public float attack;
    public State state;
    public bool isAttacking;

    //World
    public float floorSize;
    protected GameObject target;

    // Start is called before the first frame update
    protected void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();

        floorSize = GameObject.Find("Floor").GetComponent<MeshRenderer>().bounds.extents.x;

        wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset));
        wanderTicker = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        Debug.DrawLine(transform.position, wanderDestination, Color.yellow);
    }

    protected void FixedUpdate()
    {
        
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

    public abstract void Attack();

    //Author: Yuan Luo
    //Add force to rigidbody towards the target position
    //pos: the target position
    public void RigidGoTo(Vector3 pos)
    {
        rb.AddForce((pos - rb.position) * 0.03f, ForceMode.VelocityChange);
    }

    //<Helper function>
    //Auther: Yuan Luo
    //Get a random position within a circle of the instance
    //radius: the radius of the circle
    protected Vector3 GetRandomClosePosition(float radius)
    {
        Vector3 pos = Vector3.zero;

        pos = transform.position;

        float angle = Random.Range(0, 360f);
        pos.x += Mathf.Cos(angle) * radius;
        pos.z += Mathf.Sin(angle) * radius;

        while (Vector3.Distance(pos, Vector3.zero) > floorSize)
        {
            pos = transform.position;
            angle = Random.Range(0, 360f);
            pos.x += Mathf.Cos(angle) * radius;
            pos.z += Mathf.Sin(angle) * radius;
        }

        return pos;
    }

    #region Author: Rachel
    public void Seek(GameObject target)
    {
        RigidGoTo(target.transform.position);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (state == State.Wander || target == null)
            {
                target = other.gameObject;
                Debug.Log(gameObject.name + " spotted " + target.name);
            }
            else if(state == State.Pursuit)
            {
                state = State.Attack;
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (state == State.Wander && other.gameObject.tag == "Player")
        {
            state = State.Defensive;
        }
    }

    protected void OnTriggerExit(Collider other)
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
