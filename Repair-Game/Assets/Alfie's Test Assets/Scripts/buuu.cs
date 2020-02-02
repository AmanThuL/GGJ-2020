using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buuu : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //direction = new Vector3(1, 0, 0);
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
        transform.position += velocity * Time.deltaTime;
    }
}
