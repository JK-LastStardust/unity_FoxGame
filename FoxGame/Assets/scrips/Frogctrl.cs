using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogctrl : Enemy
{
    private Rigidbody2D rb;
    public LayerMask Ground;    
    public Transform leftpoint,rightpoint;
    private bool Faceleft = true;
    public float Speed,JumpForce;
    private float leftx,rightx;
    //private Animator Anim;
    private Collider2D Coll;




    protected override void Start()   //override
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();





        transform.DetachChildren();             //与子项目断绝关系
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);




    }

        void Update()
    {
        



        SwitchAnim();
    }

    void Movement()
    {
        if(base.isDestroy)
        {
            rb.velocity = new Vector2(0 , 0);
        }
        else
        {
            if(transform.position.x < leftx)
        {
            transform.localScale = new Vector3(-1,1,1);
            Faceleft = false;
        }
        else if(transform.position.x > rightx)
        {
            transform.localScale = new Vector3(1,1,1);
            Faceleft = true;
        }

        if(Coll.IsTouchingLayers(Ground))
        {
            if(Faceleft)
            {
                Anim.SetBool("jumping",true);
                rb.velocity = new Vector2(-Speed , JumpForce);
            }
            else
            {
                Anim.SetBool("jumping",true);
                rb.velocity = new Vector2(Speed , JumpForce);
            }
            
        }
        }



        
    }


    void SwitchAnim()
    {
        if(Anim.GetBool("jumping"))  //如果正在跳跃
        {
            if(rb.velocity.y < 0.1)   //若上升速度小于0.1
            {
                Anim.SetBool("jumping",false); //【jumping】关  
                Anim.SetBool("falling",true);  //【falling】开
            }   
        }
        if(Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))  //若接触地面且处于下落动画中
            {
                Anim.SetBool("falling",false);       //【falling】关
            } 
    }

    
}
