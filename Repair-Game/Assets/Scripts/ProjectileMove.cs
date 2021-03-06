﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float destroyTime;
    [SerializeField] private GameObject targetEnemy;

    public GameObject SetTargetEnemy { set => targetEnemy = value; }

    private float spawnY;

    public int damage;

    [SerializeField] private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        
        spawnY = transform.position.y;

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0 && canMove)
        {
            velocity = transform.forward * speed * Time.deltaTime;
            //transform.position += velocity * Time.deltaTime;
            //transform.rotation = Random.rotation;

            if (targetEnemy != null)
            {
                Seek(targetEnemy);
                transform.position += velocity;
            }
            else
            {
                // Shoot straight when there is no target enemy
                transform.position += velocity;
            }
        }
    }

    // Bullet doesn't when colliding with player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject obj = collision.gameObject;
            obj.GetComponent<Rigidbody>().AddForce(transform.position - obj.transform.position);
            obj.GetComponent<Enemy>().ReceiveDamage(damage);

            Destroy(gameObject);
        }

        //if (collision.collider.tag != "player")
        //{
        //    speed = 0;
        //    Destroy(gameObject);
        //}


    }

    private Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = targetPosition - transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= speed * Time.deltaTime;

        Vector3 seekingForce = desiredVelocity - velocity;
        return new Vector3(seekingForce.x, 0, seekingForce.z);
    }

    public void Seek(GameObject target)
    {
        velocity += Seek(target.transform.position);
    }

    public void SetMovable(bool value)
    {
        canMove = value;
    }
}
