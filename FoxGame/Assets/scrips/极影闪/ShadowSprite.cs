using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;              //玩家的位置
    private SpriteRenderer thisSprite;     //这个影子的图像
    private SpriteRenderer playerSprite;   //玩家的图像
    private Color color;                   //颜色
    public Color startColor;              //初始颜色

    [Header("时间控制参数")]
    public float activeTime;      //残影的显示时间
    public float activeStart;     //开始显示的时间

    [Header("不透明度控制参数")]
    private float alpha;          //不透明度  （1为100%显示,0.5为50%透明）
    public float alphaSet;        //不透明度初始值
    public float alphaMulti;      //不透明度的随时间衰减系数

    //OnEnable方法，当对象启用(active)时调用
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  //根据标签寻找到player对象
        thisSprite = GetComponent<SpriteRenderer>();   //获取影子自己身上的图像
        playerSprite = player.GetComponent<SpriteRenderer>();  //获取玩家图像
        
        alpha = alphaSet;   //设置不透明度初始值

        thisSprite.sprite = playerSprite.sprite;  //将影子图像设置为当前玩家图像

        transform.position = player.position;      //获取玩家坐标
        transform.localScale = player.localScale;  //获取玩家面向
        transform.rotation = player.rotation;      //获取玩家旋转
        

        activeStart = Time.time;    //记录影子开始显示的时间
    }

    void FixedUpdate()
    {
        alpha *= alphaMulti;   //不透明度随时间衰减
        color = new Color(startColor.r , startColor.g , startColor.b , alpha);

        thisSprite.color = color;

        if(Time.time >= activeStart + activeTime)
        {
            //若显示时间超出，返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
