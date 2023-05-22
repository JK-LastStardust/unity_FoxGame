using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Collider2D coll;
    public Collider2D colldown;     //下方碰撞器
    public int Cherry;
    public int Gem;
    public Text CherryNum;
    public Text GemNum;
    public bool isHurt;
    bool isJumping;
    int twojump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void FixedUpdate()
    {
        Movement();
        //SwitchAnim();
    }

    //移动
    void Movement()
    {
        float horizantalmove = Input.GetAxis("Horizontal");         
        float  facediretion = Input.GetAxisRaw("Horizontal");

        if(horizantalmove != 0)
        {
            rb.velocity = new Vector2(horizantalmove*speed * Time.deltaTime , rb.velocity.y); //hor的正负乘上速度来控制移动方向
        }
        if(facediretion != 0)
        {
            transform.localScale = new Vector3(facediretion , 1 ,1);
        }
        

        //跳跃
        if(Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x , jumpforce * Time.deltaTime);
        }
    }

    //动画切换
    void SwitchAnim()
    {
        anim.SetBool("idleing",false);
        //if(anim.GetBool("jumping"))
        
        if(isHurt == true)        //如果受伤
        {
            anim.SetBool("hurting",true);     //播放受伤动画
            if(Mathf.Abs(rb.velocity.x) < 4f)  //若水平方向速度绝对值小于0.1
            {
                isHurt = false;                //停止受伤动画
                anim.SetBool("hurting",false);
                anim.SetBool("idleing",true);
            }
        }
        //else if(anim.GetBool("jumping"))       
        
        else if(rb.velocity.y < -0.4)           //如果在未受伤的情况下有向下速度
        {
            anim.SetBool("jumping",false);
            anim.SetBool("falling",true);        //播放下落动画

            if(colldown.IsTouchingLayers(ground))    //判断是否碰撞地面
            {
                anim.SetBool("falling",false);
                anim.SetBool("idleing",true);
                anim.SetBool("jumping",false);
                twojump = 0;                        //跳跃次数重置
                isJumping = false;
            }
        }
        else if(colldown.IsTouchingLayers(ground))    //判断是否碰撞地面
        {
            anim.SetBool("falling",false);
            anim.SetBool("idleing",true);
            anim.SetBool("jumping",false);
            twojump = 0;                              //跳跃次数重置
            isJumping = false;
        }
    }


    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry++;
            CherryNum.text = Cherry.ToString();
        }
        if (collision.tag == "Gem")
        {
            Destroy(collision.gameObject);
            Gem++;
            GemNum.text = Gem.ToString();
        }
    }

    //敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
        {
            if(anim.GetBool("falling"))                                            
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x , jumpforce * Time.deltaTime);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)  //若角色在青蛙左边
            {
                rb.velocity = new Vector2(-10 , rb.velocity.y);                        //水平向左移动
                isHurt = true;
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)  //若角色在青蛙右边
            {
                rb.velocity = new Vector2(10 , rb.velocity.y);                        //水平向右移动
                isHurt = true;
            }      
        }
    }


}
