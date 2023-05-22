using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleCtrl : Enemy
{
    private Rigidbody2D rb;
    public LayerMask Ground;    
    public Transform  uppoint,downpoint;
    public float Speed;
    private float upy,downy;
    //private Collider2D Coll;
    private bool isup=true;
    
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        //Coll = GetComponent<Collider2D>();
        
        upy = uppoint.position.y;
        downy = downpoint.position.y;
        Destroy(uppoint.gameObject);
        Destroy(downpoint.gameObject);

    }


    void Update()
    {
        if(base.isDestroy)
        {
            rb.velocity = new Vector2(0 , 0);
        }
        else
        {
            Movement();
        }
        
    }

    void Movement()
    {
        if(isup)
        {
            rb.velocity = new Vector2(rb.velocity.x , Speed);
            if(transform.position.y > upy)
            {
                isup = false;
            }
        }else
        {
            rb.velocity = new Vector2(rb.velocity.x , -Speed);
            if(transform.position.y < downy)
            {
                isup = true;
            }
        }
    }
}
