using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravity;
    public Vector3 velocity;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20;
    public float maxXVelocity = 100;
    public float groundHeight = 5;
    public bool isGrounded = false;

    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxSpeedHoldJump = 0.4f;
    public float holdJumpTimer = 0.0f;
    public bool isDead = false;
    public float jumpGroundThreshold = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);
        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
       
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        
        if (!isGrounded)
        {
            //Debug.Log("Not Grounded");
            pos.y += velocity.y * Time.fixedDeltaTime;
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            
            else if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector3 rayOrigin = new Vector3(pos.x +7f, pos.y);
            Vector3 rayDirection = Vector3.down;
            float rayDistance = Mathf.Abs(velocity.y * Time.fixedDeltaTime);
            RaycastHit hit;
            if(Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
            {
                if(hit.collider != null)
                {
                    Platforms ground = hit.collider.GetComponent<Platforms>();
                    if ( ground != null)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        
                        isGrounded = true;
                    }
                }
            }Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            Vector3 wallSide = new Vector3(pos.x, pos.y, pos.z);
            RaycastHit wallHit;
            if (Physics.Raycast(wallSide, Vector3.right,out wallHit ,velocity.x * Time.fixedDeltaTime))
            {
                if (wallHit.collider != null)
                {
                    Platforms ground = wallHit.collider.GetComponent<Platforms>();
                    if(ground != null)
                    {
                        if (pos.y < ground.groundHeight)
                        {
                            velocity.x = 0;
                        }
                    }
                }
            }


/*
            if(pos.y <= groundHeight)
            {
                pos.y = groundHeight;
                isGrounded = true;
               
            }*/
        }
        else if (isGrounded)
        {
            //Debug.Log("grounded");
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxSpeedHoldJump * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;    
            if(velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }
            Vector3 rayOrigin = new Vector3(pos.x - 5f, pos.y);
            Vector3 rayDirection = Vector3.down;
            float rayDistance = Mathf.Abs(velocity.y * Time.fixedDeltaTime * 100);
            RaycastHit hit;
            Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance);
            
                if (hit.collider == null)
                {
                    isGrounded = false;
                    
                }
            
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);


        }


        if (pos.y < -50)
        {
            isDead = true;
        }
        if (isDead == true)
        {
            velocity.x = 0;
            velocity.y = 0;
            return;
        }
        distance += velocity.x * Time.fixedDeltaTime;
        transform.position = pos;
    }
}
