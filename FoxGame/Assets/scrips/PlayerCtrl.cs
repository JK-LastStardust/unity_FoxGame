using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;       //引用场景管理库

public class PlayerCtrl : MonoBehaviour
{
    public Collider2D coll;
    public Collider2D circleColl;
    private Animator anim;
    private Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public float sp;
    public Text CherryNum;
    public Text GemNum;
    public Text Exptext;
    public bool isHurt;
    private GameObject EnemyColl;
    public GameObject Player;
    public GameObject Event;
    private  float horizontalMove;  //角色面向
    public float playerX;
    public bool isCrouch;
    public bool isDashing;  //冲锋状态
    

    //终末改装
    public Transform groundCheck;
    public Transform UpCheck;
    public LayerMask ground;
    public bool isGround,isJump;
    public bool jumpPressed;  //按键是否按下
    public int jumpCount;     //跳跃次数
    int Cherry,Gem,Exp;
    bool isX;

    [Header("极影闪控制参数")]
    public float dashTime;       //冲锋时间
    private float dashTimeLeft;  //剩余冲锋时间
    private float lastDash = -10;      //上一次冲锋的时间点
    public float dashCD;         //冲锋技能CD
    public float dashSpeed;      //冲锋速度

    [Header("技能UI组件")]
    public Image cdImage;





    //音效组
    //public AudioSource jumpAudio;
    //public AudioSource gemAudio;
    //public AudioSource cherryAudio;
    //public AudioSource hurtAudio;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }


    void Update()     //每帧执行一次，主要用于按键判断
    {
        if(Input.GetButtonDown("Jump") && jumpCount > 0)      //若空格按下，且多段跳计数满足条件
        {
            jumpPressed = true;    //记录按键按下
        }
        Crouch();    //调用趴下函数

        if(Input.GetKeyDown(KeyCode.J))  //按下J键冲锋
        {
            Debug.Log("111");
            if(Time.time >= (lastDash + dashCD))
            {
                //可以执行冲锋
                ReadyToDash();
            }
        }

        cdImage.fillAmount -= 1.0f / dashCD * Time.deltaTime;
    }

    private void FixedUpdate()    //没每0.2秒执行一次，主要用于物理功能
    {
        Dash();  //冲刺状态方法
        if(isDashing)
        {
            return;  //如果正在冲锋，就不能执行其他移动
        }


        isGround = Physics2D.OverlapCircle(groundCheck.position , 0.1f , ground);  //判断是否在地面上
        if(isHurt)
        {
            if(!isX)
            {
                playerX = Player.transform.position.x;
                isX = true;
            }
            
            if(transform.position.x < EnemyColl.gameObject.transform.position.x)  //若角色在青蛙左边
            {
                rb.velocity = new Vector2(-10 , rb.velocity.y);                        //水平向左移动
                if(Player.transform.position.x < playerX - 5)
                {
                    isHurt = false;
                    rb.velocity = new Vector2( 0 , rb.velocity.y);
                    isX = false;
                    anim.SetBool("hurting",false);
                }
            }
            else if(transform.position.x > EnemyColl.gameObject.transform.position.x)  //若角色在青蛙右边
            {
                rb.velocity = new Vector2(10 , rb.velocity.y);                        //水平向右移动
                if(Player.transform.position.x > playerX + 5)
                {
                    isHurt = false;
                    rb.velocity = new Vector2( 0 , rb.velocity.y);
                    isX = false;
                    anim.SetBool("hurting",false);
                }
            }
        }
        else
        {
            GroundMovement();
        }
        Jump();
        SwitchAnim();
    }


    void GroundMovement()      //地面移动函数
    {
        horizontalMove = Input.GetAxisRaw("Horizontal"); //设置角色面向，返回值为-1、0、1
        rb.velocity = new Vector2(horizontalMove * speed , rb.velocity.y);   //移动

        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove , 1, 1);   //转向
        }

        
    }

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        else if(!isJump)         //自然下落
        {
            jumpCount = 1;
        }

        if(jumpPressed && isGround)
        {
            if(isCrouch)
            {
                jumpPressed = false;
            }
            else
            {
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x , jumpforce);
                //jumpAudio.Play();           //播放跳跃音效
                SoundManager.instance.JumpAudio();
                jumpCount--;
                jumpPressed = false;        //执行跳跃后标记按键为未按下，保证手感
            }
                
            
            
        }
        else if(jumpPressed && jumpCount>0 && !isCrouch)     //二段跳
        {
            if(isCrouch)
            {
                jumpPressed = false;
            }
            else
            {
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x , jumpforce);
                //jumpAudio.Play();   //播放跳跃音效
                SoundManager.instance.JumpAudio();    //调用SoundManager中的跳跃音效函数
                jumpCount--;
                jumpPressed =false;
            }
            
        }
    }
    
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
        sp = anim.GetFloat("running");
        if(isGround)
        {
            anim.SetBool("falling",false);
        }
        else if(!isGround && rb.velocity.y > 0)        //若不在地面上且向上移动
        {
            anim.SetBool("jumping",true);              //跳跃动画开
            anim.SetBool("falling",false);             //下落动画关

        }
        else if(rb.velocity.y < 0)
        {
            anim.SetBool("jumping",false);             //跳跃动画关
            anim.SetBool("falling",true);              //下落动画开
        }

        if(isHurt)
        {
            anim.SetBool("hurting",true);
            if(Mathf.Abs(rb.velocity.x) < 4f)  //若水平方向速度绝对值小于0.1
            {
                isHurt = false;                //停止受伤动画
                anim.SetBool("hurting",false);
            }
        }
    }

    //碰撞触发器
    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry++;
            //cherryAudio.Play();       //播放MCeat1音效
            SoundManager.instance.EatAudio();
            CherryNum.text = Cherry.ToString();
        }
        if (collision.tag == "Gem")
        {
            Destroy(collision.gameObject);
            Gem++;
            //gemAudio.Play();          //播放MC升级音效
            SoundManager.instance.GemAudio();
            GemNum.text = Gem.ToString();
        }

        if(collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false; //禁用所有音频
            Invoke("Restart",2f);    //延时两秒执行重置场景函数
        }
    }



    //敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jumpforce * Time.deltaTime *10
        if(collision.gameObject.tag == "Enemy")
        { 
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            EnemyColl = collision.gameObject;
            
            if(anim.GetBool("falling"))                                                //若以下落状态接触青蛙
            {
                enemy.JumpOn();                                //消灭青蛙
                Exp += 300;
                Exptext.text = Exp.ToString();


                rb.velocity = new Vector2(rb.velocity.x , jumpforce); //再次跳起
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)  //若角色在青蛙左边
            {
                //rb.velocity = new Vector2(-10000 , rb.velocity.y);                        //水平向左移动
                isHurt = true;
                //hurtAudio.Play();        //播放受伤音效
                SoundManager.instance.HurtAudio();
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)  //若角色在青蛙右边
            {
                //rb.velocity = new Vector2(10000 , rb.velocity.y);                        //水平向右移动
                isHurt = true;
                //hurtAudio.Play();         //播放受伤音效
                SoundManager.instance.HurtAudio();
            }      
        } 
    }

    void Crouch()
    {
        if(Input.GetButtonDown("Crouch"))
        {
            if(!isCrouch)
            {
                anim.SetBool("crouching",true);
                circleColl.enabled = true;
                coll.enabled = false;
                isCrouch = true;
                speed = 4;
            }
            else
            {
                if(!Physics2D.OverlapCircle(UpCheck.position,0.2f,ground))
                {
                anim.SetBool("crouching",false);
                coll.enabled = true;
                circleColl.enabled = false;
                isCrouch = false;
                speed = 8;
                }
            }
        }
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
    void ReadyToDash()   //准备冲锋的方法
    {
        isDashing = true;
        dashTimeLeft = dashTime; //重置冲刺剩余时间
        lastDash = Time.time;    //记录开始这次冲刺的时间
        cdImage.fillAmount = 1;  //设置技能CD
        SoundManager.instance.DashAudio();
    }

    void Dash()       //冲锋的方法
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)   //若剩余冲锋时间大于0
            {
                if(rb.velocity.y  > 0 && !isGround)  //若此时有一个向上的速度
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove , jumpforce);  //空中可以向上冲
                }
                rb.velocity = new Vector2(dashSpeed * horizontalMove , rb.velocity.y);

                dashTimeLeft  -= Time.deltaTime;  //剩余冲刺时间衰减

                ShadowPool.instance.GetFromPool();  //从对象池中拿出一个影子
            }

            if(dashTimeLeft <= 0)
            {
                isDashing = false;
                if(!isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove , jumpforce); 
                    //若冲刺玩还在空中，冲刺结束时会再小跳一下
                }
            }
        }
    }
        
        
        
    }


