using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public Vector3 velocity;
    [SerializeField] private int damage;

    public int Damage { get => damage; }

    // Start is called before the first frame update
    void Start()
    {
        //direction = new Vector3(1, 0, 0);
        speed = 10;
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
        transform.position += velocity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
    }

    // Player gets damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Hit by trigger");
            collision.collider.gameObject.GetComponent<Character>().GetAnimator.SetTrigger("damaged");
            collision.collider.gameObject.GetComponent<Character>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
