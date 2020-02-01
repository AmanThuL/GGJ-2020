using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    static Animator animator;
    [SerializeField] private float maxSpeed;
    private float speedBoost = 1;
    [SerializeField] private float maxSpeedBoost;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedBoost += Time.deltaTime;
            speedBoost = Mathf.Clamp(speedBoost, -1f, maxSpeedBoost);
        }
        else
        {
            if (speedBoost > 1f)
                speedBoost -= Time.deltaTime;
            speedBoost = Mathf.Clamp(speedBoost, -1f, maxSpeedBoost);
        }
        Move(y * speedBoost);
    }

    private void Move(float y)
    {
        animator.SetFloat("forward", y);
        transform.position += Vector3.forward * y * maxSpeed * speedBoost * Time.deltaTime;
    }
}
