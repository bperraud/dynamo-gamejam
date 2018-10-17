﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controler : MonoBehaviour {



    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public Transform left_check;
    public Transform right_check; 


    private bool isGrounded = true;
    
    //private Animator anim;
    private Rigidbody rb;

    private LayerMask fixedMask;
    private LayerMask jumpableMask;
    

    // Use this for initialization
    void Awake()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        fixedMask = LayerMask.GetMask("Fixed");
        jumpableMask = LayerMask.GetMask("Jumpable");
    }

    void Update()
    {
//        bool canJump = Physics.Linecast(transform.position, groundCheck.position, jumpableMask)
//                       || Physics.Linecast(transform.position, groundCheck.position, fixedMask);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        bool left_grounded = Physics.Linecast(transform.position, left_check.position, fixedMask);
        bool right_grounded = Physics.Linecast(transform.position, right_check.position, fixedMask);

        //Debug.Log("Left : " + left_grounded);
        //Debug.Log("Right : " + right_grounded);



        //anim.SetFloat("Speed", Mathf.Abs(h));

        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            if((h>0 && !right_grounded) || (h<0 && !left_grounded) )
            {
                rb.velocity = new Vector3(h * maxSpeed, rb.velocity.y, rb.velocity.z);
                //rb.velocity = new Vector3(rb.velocity.x + h * moveForce, rb.velocity.y, rb.velocity.z) ;
            }
            else{
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
        }

        //if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            //rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);




        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            //anim.SetTrigger("Jump");
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);
            jump = false;
        }
    }

    private void OnCollisionStay() {
        isGrounded = true;
    }

    private void OnCollisionExit() {
        if (isGrounded) isGrounded = false;
    }
    
    void Flip()
    {
        //facingRight = !facingRight;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }
}