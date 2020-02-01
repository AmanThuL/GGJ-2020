using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Stats
    public float moveSpeed;

    //Dash - controls dash movement 
    public float dashTicker; //if the ticker is above zero, the player is dashing
    public float dashDuration; //duration of dash
    public bool isDashing; //dash indicator
    public float dashSpeedMultiplier; //dash speed

    //Dash CD - controls dash cooldown
    public float dashCD; //cooldown between each dash
    public float dashCDTicker; //cooldown timer

    //Info
    public Vector3 position; //player position
    public Vector3 moveDirection; //direction of movement

    //Other
    public Camera mainCamera;
    void Start()
    {
        position = transform.position;
        moveSpeed = 10f;

        dashDuration = 0.3f;
        dashTicker = 0;
        isDashing = false;
        dashSpeedMultiplier = 3f;

        dashCD = 0.5f;
        dashCDTicker = 0;
    }

    void Update()
    {
        if (!isDashing)
        {
            moveDirection = Vector3.zero;
        }

        GetInput();

        DashCD();

        Dash();

        SetForward();

        SetTransform();

    }

    private void GetInput()
    {
        if (!isDashing)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += new Vector3(0, 0, -1);
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += new Vector3(-1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDirection += new Vector3(1, 0, 0);
            }

            if(dashCDTicker == 0)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    dashTicker += dashDuration;
                }
            }
            
        }

    }

    public void SetForward()
    {
        //int layerMask = 1 << 8;

        //RaycastHit hit;

        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, layerMask))
        //{
        //    if (Vector3.Distance(position, hit.point) >= 1f)
        //    {
        //        Vector3 forward = (hit.point - transform.position).normalized;
        //        forward.y = 0;
        //        transform.forward = forward;
        //    }
        //}
        if(moveDirection.magnitude != 0)
        {
            transform.forward = moveDirection;
        }
    }

    private void SetTransform()
    {
        if (isDashing)
        {
            position += moveDirection.normalized * Time.deltaTime * moveSpeed * dashSpeedMultiplier;
        }
        else
        {
            position += moveDirection.normalized * Time.deltaTime * moveSpeed;
        }

        transform.position = position;
    }

    private void Dash()
    {
        if (dashTicker > 0)
        {
            isDashing = true;
            dashTicker -= Time.deltaTime;
        }
        else
        {
            dashTicker = 0;

            if (isDashing)
            {
                dashCDTicker += dashCD;
            }

            isDashing = false;
        }
    }

    private void DashCD()
    {
        if(dashCDTicker > 0)
        {
            dashCDTicker -= Time.deltaTime;
        }
        else
        {
            dashCDTicker = 0;
        }
    }
}
