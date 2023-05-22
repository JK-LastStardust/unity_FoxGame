using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void PauseGame()
    {
        Time.timeScale = 0f;   //砸瓦鲁多
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;   //时间开始流动
    }
    
    public void SetValue(float value)   //滑动条调节音量
    {
        audioMixer.SetFloat("MainVolume", value);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }
}

