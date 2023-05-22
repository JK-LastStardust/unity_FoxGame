using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator Anim;        //保护型变量，只能在父子关系中使用    virtual:虚拟的
    protected AudioSource deathAudio;
    protected bool isDestroy;
    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void JumpOn()
        {
            Anim.SetTrigger("Death");
            deathAudio.Play();
            isDestroy = true;
        }

    public void Death()
    {
        Destroy(gameObject);
    }
}
