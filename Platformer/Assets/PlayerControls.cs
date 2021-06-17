using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 50;
    public LayerMask ropeMask;


    private Vector2 moveDir;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer rend;
    private bool grounded = false;
    private bool onRope = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            grounded = false;
        }

        if (moveDir.x > 0)
        {
            //face right
            rend.flipX = false;
        }
        else if (moveDir.x < 0)
        {
            //face left
            rend.flipX = true;
        }
        anim.SetBool("Grounded", grounded);

        RopePhysics();

    }
    void RopePhysics()
    {
        anim.SetBool("Climbing", onRope);
        
        anim.SetFloat("ClimbSpeed", rb.velocity.y);

        if (moveDir.y != 0)
        {
            Collider2D ropeCol = Physics2D.OverlapCircle(transform.position, 0.25f, ropeMask);

            if (ropeCol != null)
            {
                if (onRope == false)
                {
                    rb.velocity = Vector2.zero;
                }
                
                onRope = true;
                rb.gravityScale = 0;
                transform.position = new Vector2(ropeCol.transform.position.x, transform.position.y);
                
            }
        }
        if (onRope)
        {
            onRope = Physics2D.OverlapPoint(transform.position, ropeMask);
            if (!onRope)
            {
                rb.gravityScale = 1;
            }
        }
    }



    //Fixed Update triggers 50 times a second, regardless of framerate
    private void FixedUpdate()
    {
        rb.AddForce(Vector2.right * moveDir.x * speed);
        anim.SetFloat("WalkSpeed", Mathf.Abs(rb.velocity.x));

        if (onRope)
        {
            rb.AddForce(Vector2.up * moveDir.y * speed);
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Physics2D.OverlapPoint(transform.position + Vector3.down * 0.6f))
        {
            grounded = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }

}
