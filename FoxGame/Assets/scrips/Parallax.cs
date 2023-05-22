using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;    //摄像机位置
    public float moveRate;   //移动增幅
    private float startPointX,startPointY;
    public bool locky;//false 是否锁定y轴跟随
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
        
    }

    
    void Update()
    {
        if(locky)
        {
            transform.position = new Vector2(startPointX + Cam.position.x * moveRate,transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + Cam.position.x * moveRate , startPointY + Cam.position.y * moveRate);
        }
        
    }
}
