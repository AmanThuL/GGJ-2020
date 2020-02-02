/// @author Rudy Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float runSpeedBoost;
    [SerializeField] private bool isRunning;

    private float horMovement;
    private float vertMovement;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 forward;

    [SerializeField] private float gravityScale;

    [SerializeField] private bool isInputEnabled;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        moveDirection = Vector3.zero;
        moveSpeed = maxWalkSpeed;

        isRunning = false;
        isInputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if (isInputEnabled)
        {
            horMovement = Input.GetAxisRaw("Horizontal");
            vertMovement = Input.GetAxisRaw("Vertical");
        }
        else
        {
            horMovement = vertMovement = 0;
        }

        // Control speed
        if (!IsThereMovementInput() && !isRunning) moveSpeed = 0;
        else if (IsWalking()) moveSpeed = maxWalkSpeed;

        // Run
        if (Input.GetKey(KeyCode.LeftShift) && IsThereMovementInput())
        {
            if (!isRunning ) isRunning = true;   // set it to true if it's still false
            runSpeedBoost += Time.deltaTime;
            runSpeedBoost = Mathf.Clamp(runSpeedBoost, 1f, maxRunSpeed / maxWalkSpeed);
        }
        else
        {
            isRunning = false;
            runSpeedBoost = 1f;
        }

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
        if (!isRunning)
            animator.SetFloat("forward", moveSpeed / maxWalkSpeed);
        else
            animator.SetFloat("forward", Mathf.Lerp(1, 2, (moveSpeed * runSpeedBoost - maxWalkSpeed) / (maxRunSpeed - maxWalkSpeed)));
    }

    // Helper methods
    private bool IsThereMovementInput()
    {
        if (horMovement == 0 && vertMovement == 0)
            return false;
        return true;
    }

    private bool IsWalking()
    {
        if (IsThereMovementInput() && !isRunning)
            return true;
        return false;
    }

    public void ToggleInput(bool enabled)
    {
        isInputEnabled = enabled;
    }
}
