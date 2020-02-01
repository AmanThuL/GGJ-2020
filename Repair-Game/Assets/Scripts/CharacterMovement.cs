/// @author Rudy Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    static Animator animator;
    static CharacterController controller;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float runSpeedBoost;

    private float horMovement;
    private float vertMovement;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 forward;

    [SerializeField] private float gravityScale;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        moveDirection = Vector3.zero;
        moveSpeed = maxWalkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        horMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");

        // Control speed
        if (horMovement == 0 && vertMovement == 0) moveSpeed = 0;
        else moveSpeed = maxRunSpeed;

        // Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runSpeedBoost += Time.deltaTime;
            runSpeedBoost = Mathf.Clamp(runSpeedBoost, 1f, maxRunSpeed / maxWalkSpeed);
        }
        else runSpeedBoost = 1f;

        Rotate();
        Move();
    }

    private void Rotate()
    {
        Vector3 newForward = new Vector3(horMovement, 0f, vertMovement);
        if (newForward != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(newForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
        }
    }

    private void Move()
    {
        moveDirection = new Vector3(horMovement * moveSpeed, 0f, vertMovement * moveSpeed) * runSpeedBoost * Time.deltaTime;

        moveDirection.y += Physics.gravity.y * gravityScale;
        controller.Move(moveDirection);
        Animate();
    }

    private void Animate()
    {
        // Animation control
        animator.SetFloat("forward", moveSpeed / maxWalkSpeed);
    }

    // Helper methods
    private float sqrMagnitude_XZ(Vector3 input)
    {
        return Mathf.Sqrt(input.x * input.x + input.z * input.z);
    }
}
