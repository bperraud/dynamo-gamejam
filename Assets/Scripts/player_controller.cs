using System;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;using Object = UnityEngine.Object;public class player_controller : MonoBehaviour{    [HideInInspector] public bool facingRight = true;    [HideInInspector] public bool jump = false;    public float maxSpeed = 5f;    public float jumpForce = 1000f;    public float MaxLife = 100f;    public float deathspeed = 2f;    float currentLife;    public Slider health_bar;    public game_manager gm;    public Animator anim;    private Rigidbody rb;    public bool isSafe;    //Collision bool    bool left_fixed;    bool right_fixed;    bool rightorleft_pushable;    bool down_any;    //public Transform down_left;    //public Transform down_right;    //public Transform right_up;    //public Transform right_down;    //public Transform left_up;    //public Transform left_down;    private int fixedMask;    private int jumpableMask;    private int pushableMask;    public collider_check right;    public collider_check left;    public collider_check down;    private BoxCollider boxCollider;    GameObject object_pushing;        private void Awake()    {        currentLife = MaxLife;        rb = GetComponent<Rigidbody>();        fixedMask = LayerMask.NameToLayer("Fixed");        jumpableMask = LayerMask.NameToLayer("Jumpable");        pushableMask = LayerMask.NameToLayer("Pushable");        health_bar.maxValue = MaxLife;        isSafe = true;    }    private void Start()    {        boxCollider = GetComponent<BoxCollider>();    }    private void Update()    {        Check_Collision();        if (Input.GetButtonDown("Jump") && down_any)        {            jump = true;        }        if (isSafe)        {            currentLife = MaxLife;        }        else        {            currentLife -= Time.deltaTime * deathspeed;        }        // Debug.Log("Safe : " + safe);         health_bar.value = currentLife + 1;        if (currentLife < float.Epsilon)        {            gm.GameOver = true;            Object.Destroy(this);        }    }    public float flip_speed;    private void FixedUpdate()    {        //Flip         if (rotating)        {            current_rotation += Time.deltaTime * flip_speed;            transform.eulerAngles = new Vector3(transform.eulerAngles.x,                transform.eulerAngles.y + Time.deltaTime * flip_speed, transform.eulerAngles.z);            if (current_rotation > 180f)            {                rotating = false;                transform.eulerAngles =                    new Vector3(transform.eulerAngles.x, start_rotation + 180, transform.eulerAngles.z);            }        }        if (object_pushing != null && !down_any)        {            object_pushing.GetComponentInParent<Rigidbody>().mass = 200;        }        float h = Input.GetAxis("Horizontal");        anim.SetFloat("Speed", Mathf.Abs(h));        if ((h > 0 && !right_fixed) || (h < 0 && !left_fixed))        {            rb.velocity = new Vector3(h * maxSpeed, rb.velocity.y, rb.velocity.z);        }        if ((h > 0 && right_fixed) || (h < 0 && left_fixed))        {            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);        }        if (h > 0 && !facingRight && !rotating)            Flip();        else if (h < 0 && facingRight && !rotating)            Flip();        if (jump)        {            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);            jump = false;        }        if (down_any)        {            anim.SetBool("IsJumping", false);        }        else        {            anim.SetBool("IsJumping", true);        }        if (Mathf.Abs(rb.velocity.x) > 0.02f)        {            anim.SetBool("IsWalking", true);        }        else        {            anim.SetBool("IsWalking", false);        }    }    bool rotating;    float current_rotation;    float start_rotation;    public void Flip()    {        rotating = true;        current_rotation = 0;        facingRight = !facingRight;        start_rotation = transform.eulerAngles.y;    }    private void OnTriggerStay(Collider other)    {        if (other.CompareTag("safe_zone"))        {            isSafe = true;        }    }    private void OnTriggerExit(Collider other)    {        if (other.CompareTag("safe_zone"))        {            isSafe = false;        }        if (other.gameObject == object_pushing)        {            object_pushing.GetComponentInParent<Rigidbody>().mass = 200;        }    }    private void OnTriggerEnter(Collider other)    {        Check_Collision();        if (other.gameObject.CompareTag("Pushable") && rightorleft_pushable && down_any && !jump)        {            object_pushing = other.gameObject;            object_pushing.GetComponentInParent<Rigidbody>().mass = 1;        }        if (other.gameObject.CompareTag("WIN_ZONE"))        {            gm.Win = true;            Object.Destroy(this);        }    }    private void Check_Collision()    {        float x_size = boxCollider.size.x;        float y_size = boxCollider.size.y;        if (facingRight)        {            left_fixed = left.HasCollision && left.layer == fixedMask;            right_fixed = right.HasCollision && right.layer == fixedMask;        }        else        {            right_fixed = left.HasCollision && left.layer == fixedMask;            left_fixed = right.HasCollision && right.layer == fixedMask;        }        rightorleft_pushable = (left.HasCollision && left.layer == pushableMask) ||                               (right.HasCollision && right.layer == pushableMask);        down_any = down.HasCollision;        //Debug.Log("Pushable : "+rightorleft_pushable);        //Debug.Log("Left : " + left_fixed);        //Debug.Log("Right : " + right_fixed);        //Debug.Log("Down : " + down_any);    }}