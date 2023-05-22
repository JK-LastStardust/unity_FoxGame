using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource Button;
    public GameObject Panel;
    public GameObject Menu2;

    public void EnterAnim()
    {
        Panel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonAudio()
    {
        Button.Play();
    }

    public void Choice()
    {
        Menu2.SetActive(true);
    }

    public void Back()
    {
        Menu2.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;   //砸瓦鲁多
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;   //时间开始流动
    }
}
