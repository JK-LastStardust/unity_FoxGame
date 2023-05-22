using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;     //希望其他代码可以随时调用该类，使用单例模式

    public GameObject shadowPrefab;  //残影预制体
    
    public int shadowCount; //影子数量

    private Queue<GameObject> availableObjects = new Queue<GameObject>();  //对象池队列

    void Awake()
    {
        instance = this;  //单例模式需要在awake中创建单例

        FillPool();       //初始化对象池
    }

    public void FillPool()  //初始化对象池的方法
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);  //使生成的所有对象成为对象池物体的子集

            ReturnPool(newShadow);   //取消启用，返回对象池
            
        }
    }

    //把一个对象丢回对象池，并设置为非启用的方法，参数：要丢回去的那个对象
    public void ReturnPool(GameObject gameObject)   
    {
        gameObject.SetActive(false);  //取消启用

        availableObjects.Enqueue(gameObject);  //将这东西丢到队列的末端
    }

    //从池子里拽出来一个对象的方法
    public GameObject GetFromPool()  
    {
        //保险逻辑，若对象池内对象已空，再次填充池子
        if(availableObjects.Count == 0)
        {
            FillPool();
        }

        var outShadow = availableObjects.Dequeue();   //Dequeue:从队列的开头获取一个元素
        outShadow.SetActive(true);  //设置为启用状态

        return outShadow;
    }
}
