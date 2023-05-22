using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;   //定义 静态类 声音管理器实例

    public AudioSource audioSource;
    [SerializeField]     //使private的变量也可以在unity窗口中显示
    private AudioClip jumpAudio,hurtAudio,eatAudio,gemAudio,dashAudio;
    void Awake()
    {
        instance = this;   //铁锅炖自己 自此，变量instance = 这个SoundManager
    }

    public void JumpAudio() //跳跃音效
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio() //受伤音效
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void EatAudio() //食用音效
    {
        audioSource.clip = eatAudio;
        audioSource.Play();
    }

    public void GemAudio() //宝石音效
    {
        audioSource.clip = gemAudio;
        audioSource.Play();
    }

    public void DashAudio() //极影闪音效
    {
        audioSource.clip = dashAudio;
        audioSource.Play();
    }

    
}
